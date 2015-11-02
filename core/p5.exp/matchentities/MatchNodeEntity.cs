/*
 * Phosphorus Five, copyright 2014 - 2015, Thomas Hansen, phosphorusfive@gmail.com
 * Phosphorus Five is licensed under the terms of the MIT license, see the enclosed LICENSE file for details
 */

using System;
using p5.core;

namespace p5.exp.matchentities
{
    public class MatchNodeEntity : MatchEntity
    {
        internal MatchNodeEntity (Node node, Match match)
            : base (node, match)
        { }
        
        public override Match.MatchType TypeOfMatch {
            get { return Match.MatchType.count; }
        }
        
        public override object Value
        {
            get
            {
                object retVal = Node;
                if (!string.IsNullOrEmpty (_match.Convert)) {
                    retVal = _match.Convert == "string" ?
                        Utilities.Convert<string> (retVal, _match.Context) :
                            _match.Context.Raise ("p5.hyperlisp.get-object-value." + _match.Convert, new Node (string.Empty, retVal)).Value;
                }
                return retVal;
            }
            set
            {
                if (value == null) {
                    Node.UnTie ();
                } else {
                    var tmp = Utilities.Convert<Node> (value, _match.Context);
                    if (value is string) {
                        // Node was created from a conversion from string, making sure that we discard
                        // the automatically created "root node" in object
                        if (tmp.Count != 1)
                            throw new ApplicationException ("Tried to convert a string that would create multiple nodes to one node");
                        tmp = tmp [0];
                        Node.Replace (tmp); // ps, NOT cloned!
                    } else {
                        Node.Replace (tmp.Clone ()); // ps, cloned!
                    }
                }
            }
        }
    }
}