/*
 * phosphorus five, copyright 2014 - Mother Earth, Jannah, Gaia
 * phosphorus five is licensed as mit, see the enclosed LICENSE file for details
 */

using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using phosphorus.core;

namespace phosphorus.hyperlisp
{
    /// <summary>
    /// class to help transform between hyperlisp and <see cref="phosphorus.core.Node"/> 
    /// </summary>
    public static class hyperlisp
    {
        /// <summary>
        /// helper to transform from hyperlisp code syntax to <see cref="phosphorus.core.Node"/> tree structure
        /// </summary>
        /// <param name="context"><see cref="phosphorus.Core.ApplicationContext"/> for Active Event</param>
        /// <param name="e">parameters passed into Active Event</param>
        [ActiveEvent (Name = "pf.hyperlisp-2-nodes")]
        private static void pf_hyperlisp_2_nodes (ApplicationContext context, ActiveEventArgs e)
        {
            e.Args.AddRange (NodeBuilder.NodesFromHyperlisp (context, e.Args.Get<string> ()));
        }

        /// <summary>
        /// helper to transform from <see cref="phosphorus.core.Node"/> tree structure to hyperlisp code syntax
        /// </summary>
        /// <param name="context"><see cref="phosphorus.Core.ApplicationContext"/> for Active Event</param>
        /// <param name="e">parameters passed into Active Event</param>
        [ActiveEvent (Name = "pf.nodes-2-hyperlisp")]
        private static void pf_nodes_2_hyperlisp (ApplicationContext context, ActiveEventArgs e)
        {
            StringBuilder builder = HyperlispBuilder.Nodes2Hyperlisp (context, e.Args.Children);
            if (builder.Length == 0) {
                e.Args.Value = string.Empty;
            } else {
                string value = builder.ToString ();
                e.Args.Value = value.TrimEnd ('\r', '\n'); // getting rid of last carriage return
            }
        }

        /// <summary>
        /// helper to transform from <see cref="phosphorus.core.Node"/> tree structure to hyperlisp code syntax
        /// </summary>
        /// <param name="context"><see cref="phosphorus.Core.ApplicationContext"/> for Active Event</param>
        /// <param name="e">parameters passed into Active Event</param>
        [ActiveEvent (Name = "pf.node-2-hyperlisp")]
        private static void pf_node_2_hyperlisp (ApplicationContext context, ActiveEventArgs e)
        {
            StringBuilder builder = HyperlispBuilder.Nodes2Hyperlisp (context, new Node[] { e.Args });
            if (builder.Length == 0) {
                e.Args.Value = string.Empty;
            } else {
                string value = builder.ToString ();
                e.Args.Value = value.Substring (0, value.Length - 2); // getting rid of last carriage return
            }
        }
    }
}

