
/*
 * phosphorus five, copyright 2014 - Mother Earth, Jannah, Gaia
 * phosphorus five is licensed as mit, see the enclosed LICENSE file for details
 */

using System;
using System.IO;
using System.Web;
using System.Reflection;
using phosphorus.core;
using phosphorus.expressions;

namespace phosphorus.file
{
    /// <summary>
    /// class to help list all files within one or more folders on disc
    /// </summary>
    public static class folder_list_files
    {
        /// <summary>
        /// list all the files in folder given as value of args given
        /// </summary>
        /// <param name="context"><see cref="phosphorus.Core.ApplicationContext"/> for Active Event</param>
        /// <param name="e">parameters passed into Active Event</param>
        [ActiveEvent (Name = "pf.folder.list-files")]
        private static void pf_folder_list_files (ApplicationContext context, ActiveEventArgs e)
        {
            // retrieving root folder
            string rootFolder = common.GetRootFolder (context);

            // iterating through each folder given by caller
            XUtil.Iterate<string> (e.Args, context,
            delegate (string idx) {

                // iterating all files in current directory, and returning as nodes beneath args given
                foreach (var idxFile in Directory.GetFiles (rootFolder + idx)) {
                    e.Args.Add (new Node (string.Empty, idxFile.Replace (rootFolder, "")));
                }
            });
        }
    }
}
