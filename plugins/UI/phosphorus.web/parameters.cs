
/*
 * phosphorus five, copyright 2014 - Mother Earth, Jannah, Gaia
 * phosphorus five is licensed as mitx11, see the enclosed LICENSE file for details
 */

using System;
using System.Web;
using System.Collections.Generic;
using phosphorus.core;
using phosphorus.expressions;

namespace phosphorus.web
{
    /// <summary>
    /// helper to retrieve POST and GET parameters
    /// </summary>
    public static class parameters
    {
        /// <summary>
        /// retrieves the requested POST or GET parameter(s)
        /// </summary>
        /// <param name="context"><see cref="phosphorus.Core.ApplicationContext"/> for Active Event</param>
        /// <param name="e">parameters passed into Active Event</param>
        [ActiveEvent (Name = "pf.web.parameters.get")]
        private static void pf_web_parameters_get (ApplicationContext context, ActiveEventArgs e)
        {
            // looping through each parameter requested by caller
            foreach (var idx in XUtil.Iterate<string> (e.Args, context)) {

                // adding parameter's name/value as Node return value
                if (HttpContext.Current.Request.Params [idx] != null)
                    e.Args.Add (idx, HttpContext.Current.Request [idx]);
            }
        }

        /// <summary>
        /// lists all parameters in request
        /// </summary>
        /// <param name="context"><see cref="phosphorus.Core.ApplicationContext"/> for Active Event</param>
        /// <param name="e">parameters passed into Active Event</param>
        [ActiveEvent (Name = "pf.web.parameters.list")]
        private static void pf_web_parameters_list (ApplicationContext context, ActiveEventArgs e)
        {
            CollectionBase.List (e.Args, context, delegate { return HttpContext.Current.Request.Params.AllKeys; });
        }
    }
}