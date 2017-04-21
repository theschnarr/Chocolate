using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomacy.Kit
{
    /// <summary>
    /// Details the ambassadors and request types required to fully process a request.
    /// </summary>
    public class RouteInfo
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public RouteInfo()
        {
            Stages = new Queue<RouteStage>();
        }
        /// <summary>
        /// Type: <see cref="Queue{RouteStage}"/><para>Lists the stages, in order, that should be followed when processing the associated request.</para>
        /// </summary>
        public Queue<RouteStage> Stages { get; private set; }
        /// <summary>
        /// Adds the new <paramref name="stage"/> to the <see cref="Stages"/> queue.
        /// </summary>
        /// <param name="stage">Type: <see cref="RouteStage"/><para>The new stage to be added to the queue.</para></param>
        public void AddStage(RouteStage stage)
        {
            Stages.Enqueue(stage);
        }
    }
    /// <summary>
    /// Details the ID and action that should be used to process the request at this stage.
    /// </summary>
    public class RouteStage
    {
        /// <summary>
        /// Type: <see cref="String"/><para>The ID of the ambassador that should be used to process the request at this stage.</para>
        /// </summary>
        public string ID { get; private set; }
        /// <summary>
        /// Type: <see cref="String"/><para>The key associated with the action that the <paramref name="ID">ambassador</paramref> should perform.</para>
        /// </summary>
        public string Action { get; private set; }
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public RouteStage()
        {
            ID = "";
            Action = "";
        }
        /// <summary>
        /// Creates a new instance of <see cref="RouteStage"/> with the <paramref name="id"/> and <paramref name="action"/>.
        /// </summary>
        /// <param name="id">Type: <see cref="String"/><para>The ID of the ambassador that should process this stage.</para></param>
        /// <param name="action">Type: <see cref="String"/><para>The name of the action that should be processed at this stage.</para></param>
        public RouteStage(string  id, string action)
        {
            ID = id;
            Action = action;
        }
    }
}
