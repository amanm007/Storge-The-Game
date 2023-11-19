using UnityEngine;
using System.Collections.Generic;
#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

namespace Pathfinding {
	
	public class Seeker : VersionedMonoBehaviour {
		
		public bool drawGizmos = true;

	
		public bool detailedGizmos;

		/// <summary>Path modifier which tweaks the start and end points of a path</summary>
		[HideInInspector]
		public StartEndModifier startEndModifier = new StartEndModifier();

		
		[HideInInspector]
		public int traversableTags = -1;

		[HideInInspector]
		public int[] tagPenalties = new int[32];

		
		///
		/// If you know the name of the graph you can use the <see cref="Pathfinding.GraphMask.FromGraphName"/> method:
	
		
		/// </summary>
		[HideInInspector]
		public GraphMask graphMask = GraphMask.everything;

		/// <summary>Used for serialization backwards compatibility</summary>
		[UnityEngine.Serialization.FormerlySerializedAs("graphMask")]
		int graphMaskCompatibility = -1;

		public OnPathDelegate pathCallback;

		/// <summary>Called before pathfinding is started</summary>
		public OnPathDelegate preProcessPath;

		public OnPathDelegate postProcessPath;

		/// <summary>Used for drawing gizmos</summary>
		[System.NonSerialized]
		List<Vector3> lastCompletedVectorPath;

		
		[System.NonSerialized]
		List<GraphNode> lastCompletedNodePath;

		/// <summary>The current path</summary>
		[System.NonSerialized]
		protected Path path;

		/// <summary>Previous path. Used to draw gizmos</summary>
		[System.NonSerialized]
		private Path prevPath;

		/// <summary>Cached delegate to avoid allocating one every time a path is started</summary>
		private readonly OnPathDelegate onPathDelegate;

		/// <summary>Temporary callback only called for the current path. This value is set by the StartPath functions</summary>
		private OnPathDelegate tmpPathCallback;

		/// <summary>The path ID of the last path queried</summary>
		protected uint lastPathID;

		/// <summary>Internal list of all modifiers</summary>
		readonly List<IPathModifier> modifiers = new List<IPathModifier>();

		public enum ModifierPass {
			PreProcess,
			// An obsolete item occupied index 1 previously
			PostProcess = 2,
		}

		public Seeker () {
			onPathDelegate = OnPathComplete;
		}

		/// <summary>Initializes a few variables</summary>
		protected override void Awake () {
			base.Awake();
			startEndModifier.Awake(this);
		}

		public Path GetCurrentPath () {
			return path;
		}

		
		/// <param name="pool">If true then the path will be pooled when the pathfinding system is done with it.</param>
		public void CancelCurrentPathRequest (bool pool = true) {
			if (!IsDone()) {
				path.FailWithError("Canceled by script (Seeker.CancelCurrentPathRequest)");
				if (pool) {
					path.Claim(path);
					path.Release(path);
				}
			}
		}

		
		
		/// Calls OnDestroy on the <see cref="startEndModifier"/>.
		///
		/// See: <see cref="ReleaseClaimedPath"/>
		/// See: <see cref="startEndModifier"/>
		/// </summary>
		public void OnDestroy () {
			ReleaseClaimedPath();
			startEndModifier.OnDestroy(this);
		}

		
		void ReleaseClaimedPath () {
			if (prevPath != null) {
				prevPath.Release(this, true);
				prevPath = null;
			}
		}

		/// <summary>Called by modifiers to register themselves</summary>
		public void RegisterModifier (IPathModifier modifier) {
			modifiers.Add(modifier);

			// Sort the modifiers based on their specified order
			modifiers.Sort((a, b) => a.Order.CompareTo(b.Order));
		}

		/// <summary>Called by modifiers when they are disabled or destroyed</summary>
		public void DeregisterModifier (IPathModifier modifier) {
			modifiers.Remove(modifier);
		}

		public void PostProcess (Path path) {
			RunModifiers(ModifierPass.PostProcess, path);
		}

		/// <summary>Runs modifiers on a path</summary>
		public void RunModifiers (ModifierPass pass, Path path) {
			if (pass == ModifierPass.PreProcess) {
				if (preProcessPath != null) preProcessPath(path);

				for (int i = 0; i < modifiers.Count; i++) modifiers[i].PreProcess(path);
			} else if (pass == ModifierPass.PostProcess) {
				Profiler.BeginSample("Running Path Modifiers");
				// Call delegates if they exist
				if (postProcessPath != null) postProcessPath(path);

				// Loop through all modifiers and apply post processing
				for (int i = 0; i < modifiers.Count; i++) modifiers[i].Apply(path);
				Profiler.EndSample();
			}
		}

		
		public bool IsDone () {
			return path == null || path.PipelineState >= PathState.Returned;
		}

		
		void OnPathComplete (Path path) {
			OnPathComplete(path, true, true);
		}

		
		void OnPathComplete (Path p, bool runModifiers, bool sendCallbacks) {
			if (p != null && p != path && sendCallbacks) {
				return;
			}

			if (this == null || p == null || p != path)
				return;

			if (!path.error && runModifiers) {
				// This will send the path for post processing to modifiers attached to this Seeker
				RunModifiers(ModifierPass.PostProcess, path);
			}

			if (sendCallbacks) {
				p.Claim(this);

				lastCompletedNodePath = p.path;
				lastCompletedVectorPath = p.vectorPath;

				// This will send the path to the callback (if any) specified when calling StartPath
				if (tmpPathCallback != null) {
					tmpPathCallback(p);
				}

				// This will send the path to any script which has registered to the callback
				if (pathCallback != null) {
					pathCallback(p);
				}

				
				if (prevPath != null) {
					prevPath.Release(this, true);
				}

				prevPath = p;
			}
		}


		
		
		/// This path can be sent to <see cref="StartPath(Path,OnPathDelegate,int)"/> with no change, but if no change is required <see cref="StartPath(Vector3,Vector3,OnPathDelegate)"/> does just that.
		
		[System.Obsolete("Use ABPath.Construct(start, end, null) instead")]
		public ABPath GetNewPath (Vector3 start, Vector3 end) {
			// Construct a path with start and end points
			return ABPath.Construct(start, end, null);
		}

		
		/// Since this method does not take a callback parameter, you should set the <see cref="pathCallback"/> field before calling this method.
		/// </summary>
		/// <param name="start">The start point of the path</param>
		/// <param name="end">The end point of the path</param>
		public Path StartPath (Vector3 start, Vector3 end) {
			return StartPath(start, end, null);
		}

	
		/// <param name="start">The start point of the path</param>
		/// <param name="end">The end point of the path</param>
		/// <param name="callback">The function to call when the path has been calculated</param>
		public Path StartPath (Vector3 start, Vector3 end, OnPathDelegate callback) {
			return StartPath(ABPath.Construct(start, end, null), callback);
		}

		/// <param name="start">The start point of the path</param>
		/// <param name="end">The end point of the path</param>
		/// <param name="callback">The function to call when the path has been calculated</param>
		/// <param name="graphMask">Mask used to specify which graphs should be searched for close nodes. See #Pathfinding.NNConstraint.graphMask. This will override #graphMask for this path request.</param>
		public Path StartPath (Vector3 start, Vector3 end, OnPathDelegate callback, GraphMask graphMask) {
			return StartPath(ABPath.Construct(start, end, null), callback, graphMask);
		}

		
		/// <param name="p">The path to start calculating</param>
		/// <param name="callback">The function to call when the path has been calculated</param>
		public Path StartPath (Path p, OnPathDelegate callback = null) {
			
			if (p.nnConstraint.graphMask == -1) p.nnConstraint.graphMask = graphMask;
			StartPathInternal(p, callback);
			return p;
		}

		
		/// <param name="p">The path to start calculating</param>
		/// <param name="callback">The function to call when the path has been calculated</param>
		/// <param name="graphMask">Mask used to specify which graphs should be searched for close nodes. See #Pathfinding.GraphMask. This will override #graphMask for this path request.</param>
		public Path StartPath (Path p, OnPathDelegate callback, GraphMask graphMask) {
			p.nnConstraint.graphMask = graphMask;
			StartPathInternal(p, callback);
			return p;
		}

		/// <summary>Internal method to start a path and mark it as the currently active path</summary>
		void StartPathInternal (Path p, OnPathDelegate callback) {
			p.callback += onPathDelegate;

			p.enabledTags = traversableTags;
			p.tagPenalties = tagPenalties;

			// Cancel a previously requested path is it has not been processed yet and also make sure that it has not been recycled and used somewhere else
			if (path != null && path.PipelineState <= PathState.Processing && path.CompleteState != PathCompleteState.Error && lastPathID == path.pathID) {
				path.FailWithError("Canceled path because a new one was requested.\n"+
					"This happens when a new path is requested from the seeker when one was already being calculated.\n" +
					"For example if a unit got a new order, you might request a new path directly instead of waiting for the now" +
					" invalid path to be calculated. Which is probably what you want.\n" +
					"If you are getting this a lot, you might want to consider how you are scheduling path requests.");
				// No callback will be sent for the canceled path
			}

			// Set p as the active path
			path = p;
			tmpPathCallback = callback;

			// Save the path id so we can make sure that if we cancel a path (see above) it should not have been recycled yet.
			lastPathID = path.pathID;

			// Pre process the path
			RunModifiers(ModifierPass.PreProcess, path);

			// Send the request to the pathfinder
			AstarPath.StartPath(path);
		}


		/// <summary>Draws gizmos for the Seeker</summary>
		public void OnDrawGizmos () {
			if (lastCompletedNodePath == null || !drawGizmos) {
				return;
			}

			if (detailedGizmos) {
				Gizmos.color = new Color(0.7F, 0.5F, 0.1F, 0.5F);

				if (lastCompletedNodePath != null) {
					for (int i = 0; i < lastCompletedNodePath.Count-1; i++) {
						Gizmos.DrawLine((Vector3)lastCompletedNodePath[i].position, (Vector3)lastCompletedNodePath[i+1].position);
					}
				}
			}

			Gizmos.color = new Color(0, 1F, 0, 1F);

			if (lastCompletedVectorPath != null) {
				for (int i = 0; i < lastCompletedVectorPath.Count-1; i++) {
					Gizmos.DrawLine(lastCompletedVectorPath[i], lastCompletedVectorPath[i+1]);
				}
			}
		}

		protected override int OnUpgradeSerializedData (int version, bool unityThread) {
			if (graphMaskCompatibility != -1) {
				Debug.Log("Loaded " + graphMaskCompatibility + " " + graphMask.value);
				graphMask = graphMaskCompatibility;
				graphMaskCompatibility = -1;
			}
			return base.OnUpgradeSerializedData(version, unityThread);
		}
	}
}
