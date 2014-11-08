/*
 * phosphorus five, copyright 2014 - Mother Earth, Jannah, Gaia
 * phosphorus five is licensed as mit, see the enclosed LICENSE file for details
 */

using System;
using System.Collections.Generic;

namespace phosphorus.core
{
    /// <summary>
    /// arguments passed into and returned from Active Events
    /// </summary>
    [Serializable]
    public class Node
    {
        /// <summary>
        /// DNA code for Node
        /// </summary>
        public class DNA : IComparable
        {
            internal List<int> _value;

            private DNA ()
            {
                _value = new List<int> ();
            }

            internal DNA (Node node)
                : this ()
            {
                Node idxNode = node;
                while (idxNode._parent != null) {
                    for (int idxNo = 0; idxNo < idxNode._parent._children.Count; idxNo ++) {
                        if (idxNode == idxNode._parent._children [idxNo]) {
                            _value.Add (idxNo);
                            break;
                        }
                    }
                    idxNode = idxNode._parent;
                }
            }

            /// <summary>
            /// returns the depth of the DNA, meaning the number of ancestor nodes from root node the current node has
            /// </summary>
            /// <value>depth</value>
            public int Count {
                get {
                    return _value.Count;
                }
            }

            /// <summary>
            /// compares two nodes for equality
            /// </summary>
            /// <param name="lhs">left hand side node</param>
            /// <param name="rhs">right hand side node</param>
            public static bool operator == (DNA lhs, DNA rhs)
            {
                return lhs.CompareTo (rhs) == 0;
            }

            /// <summary>
            /// compares two nodes for not-equality
            /// </summary>
            /// <param name="lhs">left hand side node</param>
            /// <param name="rhs">right hand side node</param>
            public static bool operator != (DNA lhs, DNA rhs)
            {
                return lhs.CompareTo (rhs) != 0;
            }

            /// <summary>
            /// compares two nodes to see if left hand side is more than right hand side node
            /// </summary>
            /// <param name="lhs">left hand side node</param>
            /// <param name="rhs">right hand side node</param>
            public static bool operator > (DNA lhs, DNA rhs)
            {
                return lhs.CompareTo (rhs) == 1;
            }

            /// <summary>
            /// compares two nodes to see if left hand side is less than right hand side node
            /// </summary>
            /// <param name="lhs">left hand side node</param>
            /// <param name="rhs">right hand side node</param>
            public static bool operator < (DNA lhs, DNA rhs)
            {
                return lhs.CompareTo (rhs) == -1;
            }

            /// <summary>
            /// compares two nodes to see if left hand side is more than or equal to right hand side node
            /// </summary>
            /// <param name="lhs">left hand side node</param>
            /// <param name="rhs">right hand side node</param>
            public static bool operator >= (DNA lhs, DNA rhs)
            {
                return lhs.CompareTo (rhs) != -1;
            }

            /// <summary>
            /// compares two nodes to see if left hand side is less than or equal to right hand side node
            /// </summary>
            /// <param name="lhs">left hand side node</param>
            /// <param name="rhs">right hand side node</param>
            public static bool operator <= (DNA lhs, DNA rhs)
            {
                return lhs.CompareTo (rhs) != 1;
            }

            /// <summary>
            /// returns the logical AND of the given DNA codes.  basically finds the common ancestor's DNA code
            /// </summary>
            /// <param name="lhs">left hand side node</param>
            /// <param name="rhs">right hand side node</param>
            public static DNA operator & (DNA lhs, DNA rhs)
            {
                DNA retVal = new DNA ();
                for (int idxNo = 0; idxNo < lhs._value.Count && idxNo < rhs._value.Count; idxNo++) {
                    if (lhs._value [idxNo].CompareTo (rhs._value [idxNo]) == 0)
                        retVal._value.Add (idxNo);
                    else
                        break;
                }
                return retVal;
            }

            public override bool Equals (object obj)
            {
                if (!(obj is DNA))
                    return false;
                return _value.Equals (((DNA)obj)._value);
            }

            public override int GetHashCode ()
            {
                return _value.GetHashCode ();
            }

            public override string ToString ()
            {
                string tmp = "";
                foreach (int idx in _value) {
                    tmp += "-" + idx;
                }
                tmp = tmp.Trim (new char[] { '-' });
                return string.Format ("[DNA: Value={0}]", tmp);
            }

            public int CompareTo (object obj)
            {
                if (obj == null || !(obj is DNA))
                    throw new ArgumentException ("cannot compare DNA to: " + (obj ?? "[null]").ToString ());
                DNA rhs = obj as DNA;

                for (int idxNo = 0; idxNo < _value.Count && idxNo < rhs._value.Count; idxNo ++) {
                    int cmpVals = _value [idxNo].CompareTo (rhs._value [idxNo]);
                    if (cmpVals != 0)
                        return cmpVals;
                }
                if (_value.Count > rhs._value.Count)
                    return 1;
                if (_value.Count < rhs._value.Count)
                    return -1;
                return 0;
            }
        }

        private List<Node> _children;
        private Node _parent;

        /// <summary>
        /// initializes a new instance of the <see cref="phosphorus.core.Node"/> class
        /// </summary>
        public Node ()
        {
            _children = new List<Node> ();
        }

        /// <summary>
        /// initializes a new instance of the <see cref="phosphorus.core.Node"/> class
        /// </summary>
        /// <param name="name">name of node</param>
        public Node (string name)
            : this ()
        {
            Name = name;
        }

        /// <summary>
        /// initializes a new instance of the <see cref="phosphorus.core.Node"/> class
        /// </summary>
        /// <param name="name">name of node</param>
        /// <param name="value">value of node</param>
        public Node (string name, string value)
            : this (name)
        {
            Value = value;
        }

        /// <summary>
        /// gets or sets the name of the node
        /// </summary>
        /// <value>the name</value>
        public string Name {
            get;
            set;
        }

        /// <summary>
        /// gets or sets the value of the node
        /// </summary>
        /// <value>the value</value>
        public object Value {
            get;
            set;
        }

        /// <summary>
        /// returns the number of chilren nodes
        /// </summary>
        /// <value>number of children</value>
        public int Count {
            get {
                return _children.Count;
            }
        }

        public T Get<T> ()
        {
            if (Value == null)
                return default (T);

            if (typeof(T) == Value.GetType ())
                return (T)Value;

            return (T)Convert.ChangeType (Value, typeof(T), System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// gets the children of this instance
        /// </summary>
        /// <value>its children</value>
        public IEnumerable<Node> Children {
            get {
                return _children;
            }
        }

        /// <summary>
        /// returns DNA code for Node
        /// </summary>
        /// <value>the DNA code, or position in Node tree</value>
        public DNA Position {
            get {
                return new DNA (this);
            }
        }

        /// <summary>
        /// finds a node according to the given <see cref="phosphorus.core.Node+DNA"/>
        /// </summary>
        /// <param name="dna">dna of node to find</param>
        public Node Find (DNA dna)
        {
            Node idxNode = this;
            while (idxNode._parent != null)
                idxNode = idxNode._parent;
            foreach (var idxNo in dna._value) {
                idxNode = idxNode [idxNo];
            }
            return idxNode;
        }

        /// <summary>
        /// returns the first child of the node
        /// </summary>
        /// <value>the first child</value>
        public Node FirstChild {
            get {
                if (_children.Count > 0)
                    return _children [0];
                return null;
            }
        }

        /// <summary>
        /// returns the last child of the node
        /// </summary>
        /// <value>the last child</value>
        public Node LastChild {
            get {
                if (_children.Count > 0)
                    return _children [_children.Count - 1];
                return null;
            }
        }

        /// <summary>
        /// returns the next sibling of the current node
        /// </summary>
        /// <value>the next sibling</value>
        public Node NextSibling {
            get {
                if (_parent == null)
                    return null;
                int idxNo = 0;
                foreach (Node idxNode in _parent._children) {
                    if (idxNode == this)
                        break;
                    idxNo += 1;
                }
                idxNo += 1;
                if (idxNo < _parent._children.Count)
                    return _parent._children [idxNo];
                return null;
            }
        }

        /// <summary>
        /// returns the previous sibling of the current node
        /// </summary>
        /// <value>the previous sibling</value>
        public Node PreviousSibling {
            get {
                if (_parent == null)
                    return null;
                int idxNo = 0;
                foreach (Node idxNode in _parent._children) {
                    if (idxNode == this)
                        break;
                    idxNo += 1;
                }
                idxNo -= 1;
                if (idxNo >= 0)
                    return _parent._children [idxNo];
                return null;
            }
        }

        /// <summary>
        /// returns the root node of the tree
        /// </summary>
        /// <value>the root node</value>
        public Node Root {
            get {
                Node idxNode = this;
                while (idxNode._parent != null)
                    idxNode = idxNode._parent;
                return idxNode;
            }
        }

        /// <summary>
        /// adds a child node to its children collection
        /// </summary>
        /// <param name="node">node to add</param>
        public void Add (Node node)
        {
            node._parent = this;
            _children.Add (node);
        }

        /// <summary>
        /// adds a range of nodes
        /// </summary>
        /// <param name="nodes">nodes to add</param>
        public void AddRange (IEnumerable<Node> nodes)
        {
            foreach (Node idxNode in nodes) {
                Add (idxNode);
            }
        }

        /// <summary>
        /// gets or sets the <see cref="phosphorus.core.Node"/> with the specified index
        /// </summary>
        /// <param name="index">index of node to retrieve or set</param>
        public Node this [int index]
        {
            get {
                return _children [index];
            }
            set {
                _children [index]._parent = null;
                value._parent = this;
                _children [index] = value;
            }
        }
    }
}
