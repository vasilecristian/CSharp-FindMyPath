using System;
using System.Collections.Generic;

namespace fmp
{
    /// <summary>
    /// This is a ticket used to describe a find path request.
    /// </summary>
    public class Ticket
    {

		/// <summary>
		/// Used to describe the possible states of the Ticket.
		/// </summary>
		public enum STATE
		{
			/// <summary>
			/// The ticket is waiting to be processed.
			/// </summary>
			WAITING = 0,

			/// <summary>
			/// The ticket is processed...
			/// </summary>
			PROCESSING,

			/// <summary>
			/// The ticket was processed with success.
			/// </summary>
			COMPLETED,

			/// <summary>
			/// The ticket was processed but the process was stopped for some reason...
			/// </summary>
			STOPPED,
		};

		/// <summary>
        /// It is the state of this ticket. By default is in WAITING state.
        /// </summary>
		public STATE State { get; internal set; } = STATE.WAITING;


		/// <summary>
		/// Getter for the steps required to determine the path.
		/// </summary>
		public uint Steps { get; set; } = 0;

		/// <summary>
		/// Getter for the detected path.
		/// </summary>
		public List<uint> FoundedPath { get; internal set; }

		/// <summary>
		/// Getter for the goal node.
		/// </summary>
		public uint GoalIndex { get; internal set; }

		/// <summary>
		/// Getter for the start node.
		/// </summary>
		public uint StartIndex { get; internal set; }


		/// <summary>
		/// Getter for the opened list.
		/// </summary>
		internal List<Node> OpenedList { get; set; } = null;

		/// <summary>
		/// Getter for the closed list.
		/// </summary>
		internal List<Node> ClosedList { get; set; } = null;


		/// <summary>
        /// It is the current procesed Node.
        /// </summary>
		internal Node Current { get; set; } = null;


		public Ticket(uint startIndex, uint goalIndex)
        {
			StartIndex = startIndex;
			GoalIndex = goalIndex;
        }
    }
}
