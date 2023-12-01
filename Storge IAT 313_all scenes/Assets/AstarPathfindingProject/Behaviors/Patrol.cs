using UnityEngine;
using System.Collections;

namespace Pathfinding {
	/// <summary>
	/// Simple patrol behavior.
	/// This will set the destination on the agent so that it moves through the sequence of objects in the <see cref="targets"/> array.
	/// Upon reaching a target it will wait for <see cref="delay"/> seconds.
	///
	/// See: <see cref="Pathfinding.AIDestinationSetter"/>
	/// See: <see cref="Pathfinding.AIPath"/>
	/// See: <see cref="Pathfinding.RichAI"/>
	/// See: <see cref="Pathfinding.AILerp"/>
	/// </summary>
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_patrol.php")]
	public class Patrol : VersionedMonoBehaviour {
		/// <summary>Target points to move to in order</summary>
		public Transform[] targets;

		/// <summary>Time in seconds to wait at each target</summary>
		public float delay = 0;

		/// <summary>Current target index</summary>
		int index;

		IAstarAI agent;
		float switchTime = float.PositiveInfinity;

		protected override void Awake () {
			base.Awake();
			agent = GetComponent<IAstarAI>();
        }

        /// <summary>Update is called once per frame</summary>
        void Update()
        {
            if (targets.Length == 0) return;

            bool search = false;

            // Note: using reachedEndOfPath and pathPending instead of reachedDestination here because
            // if the destination cannot be reached by the agent, we don't want it to get stuck, we just want it to get as close as possible and then move on.
            if (agent.reachedEndOfPath && !agent.pathPending && float.IsPositiveInfinity(switchTime))
            {
                switchTime = Time.time + delay;
            }

            if (Time.time >= switchTime)
            {
                index = index + 1;
                search = true;
                switchTime = float.PositiveInfinity;
            }

            index = index % targets.Length;
            agent.destination = targets[index].position;

            if (search) agent.SearchPath();
        }
        public void UpdateTargets(Transform[] newTargets)
        {
            targets = newTargets;
            index = 0; // Reset index to start patrolling from the first new target
            switchTime = float.PositiveInfinity; // Reset switch time
        }
        public Vector2 GetNextPatrolPoint()
        {
            if (targets.Length == 0) return Vector2.zero;

            // Ensure the index is within the bounds of the targets array
            int nextIndex = (index + 1) % targets.Length;
            return targets[nextIndex].position;
        }
        public Vector2 GetCurrentPatrolPoint()
        {
            if (targets.Length == 0 || index >= targets.Length) return Vector2.zero;
            return targets[index].position;
        }

    }
}
