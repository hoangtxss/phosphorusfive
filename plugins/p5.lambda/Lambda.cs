/*
 * Phosphorus Five, copyright 2014 - 2015, Thomas Hansen, phosphorusfive@gmail.com
 * Phosphorus Five is licensed under the terms of the MIT license, see the enclosed LICENSE file for details
 */

using System.Collections.Generic;
using System.Linq;
using p5.core;
using p5.exp;
using p5.exp.exceptions;

/// <summary>
///     Main namespace for p5.lambda keywords.
/// 
///     Contains all the keywords in p5.lambda, such as [while], [for-each] and [lambda]. p5.lambda is a graph object transformation programming
///     <em>"language"</em>, which means it allows you to modify and transform tree-structures, and execute these as execution trees. p5.lambda
///     is based upon Active Events, which is a design pattern that allows you to dynamically tie together statically created programming modules,
///     created for instance in C#, such as is the case with p5.lambda. Below is an example of a simple p5.lambda program;
/// 
///     <pre>_foo
///   bar1:x
///   bar2:y
/// _dest
/// set:@/-?value
///   source:@/./-2/"*"?value</pre>
/// 
///     The above program, simply transforms the values of all nodes beneath the [_foo] node, and creates a single string value from them.
/// 
///     p5.lambda is not a <em>"general purpose"</em> programming language, but should rather be though of as an <em>"orchestration programming
///     language"</em>, allowing you to loosely tie together existing functionality, in a dynamic programming environment. This just happens to
///     be one of the largest problems in today's world of System Development and System Architecture. However, to illustrate the point that you 
///     cannot use p5.lambda for all your concerns, realize that p5.lamba cannot even add two numbers. If you tried to add 2+2 in p5.lambda, it would
///     actually yield the result of <em>"22"</em>.
/// 
///     When it comes to <em>"orchestrating"</em> your application together though, p5.lambda is probably a superior choice, since it allows you
///     to very loosely couple together existing functionality, from different modules, without bringing in any dependencies between your modules, 
///     what-so-ever. This trait of p5.lambda, is the reasons why we say that p5.lambda <em>"enables Agile Programming"</em>.
/// 
///     p5.lambda is your <em>"front-end programming language"</em> you might say, because it allows you to <em>"tie together"</em> functionality
///     written in other languages, within a very loosely coupled architecture. Phosphorus Five contains Active Events for a lot of the tasks you'd normally
///     need to write your own plugins for though, which means that you actually very seldom need to resort to C# or VB.NET to extend your 
///     Phosphorus Five installation. When you do however, you will fast recognize, that the Active Event design pattern, gives you a superior
///     bridge between p5.lambda and C#.
/// 
///     In fact, if you study p5.lambda, and its implementation, you will recognize that p5.lambda <em>"hardly exists"</em>, since it's really
///     nothing but loosely coupled Active Events in itself. For instance the [for-each] keyword, or the [if] keyword in p5.lambda, are actually
///     Active Events, that knows nothing about each other. This allows you to easily extend p5.lambda with your own keywords, as you see fit.
/// 
///     And in fact, there are no semantic differences between any Active Event, such as [load-file], and a <em>"keyword"</em> in p5.lambda.
/// </summary>
namespace p5.lambda
{
    /// <summary>
    ///     Class wrapping all [lambda.xxx] keywords in p5.lambda.
    /// 
    ///     The [lambda.xxx] keywords allows you to execute some specific piece of p5.lambda code.
    /// </summary>
    public static class Lambda
    {
        private delegate void ExecuteFunctor (ApplicationContext context, Node exe, IEnumerable<Node> args);

        private static void Executor (ExecuteFunctor functor, ApplicationContext context, Node args, string lambdaEvent)
        {
            if (args.Name == lambdaEvent && args.Value != null) {

                if (XUtil.IsExpression (args.Value)) {

                    // executing a value object, converting to node, before we pass into execution method,
                    // making sure we pass in children of [lambda] as "arguments" or "parameters" to [lambda] statement
                    var match = (args.Value as Expression).Evaluate (args, context);
                    foreach (var idxSource in match) {
                        functor (context, Utilities.Convert<Node> (idxSource.Value, context), args.Children);
                    }
                } else {
                    var lambda = context.Raise ("lisp2lambda", new Node (string.Empty, args.Get<string> (context)));
                    functor (context, lambda, args.Children);
                }
            } else {

                // executing current scope
                functor (context, args, new Node[] {});
            }
        }

        /// <summary>
        ///     Executes a specified piece of p5.lambda block.
        /// 
        ///     The [lambda] keyword can either take a list of children nodes, or an 
        ///     <see cref="phosphorus.expressions.Expression">Expression</see> as its source, or both. If you have no value in your
        ///     [lambda] node, then the children nodes of your [lambda] node will be executed as p5.lambda nodes. For instance;
        /// 
        ///     <pre>lambda
        ///   set:@/.?value
        ///     source:success</pre>
        /// 
        ///     You can alternatively invoke [lambda] by giving it an expression, instead of a list of children nodes. If you do, then
        ///     the result of that expression will be executed instead of its children nodes, passing in any children as <em>"arguments"</em>
        ///     to your p5.lambda execution. For instance;
        /// 
        ///     <pre>_exe
        ///   set:@/.?value
        ///     source:Hello {0}
        ///       :@/./././"*"/_input?value
        /// lambda:@/-?node
        ///   _input:Thomas</pre>
        /// 
        ///     Please notice in the above code, that you do not supply an expression leading directly to the nodes that you wish to execute, 
        ///     but instead the expression leads to the node that <em>"wraps"</em> your p5.lambda nodes to be executed. Meaning, [_exe] for the
        ///     above example. You can also of course supply an expression yielding multiple results, for instance;
        /// 
        ///     <pre>_exe1
        ///   set:@/.?value
        ///     source:Hello {0}
        ///       :@/./././"*"/_input?value
        /// _exe2
        ///   set:@/.?value
        ///     source:Howdy {0}
        ///       :@/./././"*"/_input?value
        /// lambda:@/-|/-2?node
        ///   _input:Thomas</pre>
        /// 
        ///     Anything you submit to [lambda] that is not a node-set, will be converted into a node-set, before executed. Consider the 
        ///     following code;
        /// 
        ///     <pre>lambda:\@"set:@/./""*""/_result/#?name
        ///   source:Hello there {0}
        ///     :@/./././""*""/_input?value"
        ///   _result:node:
        ///   _input:Thomas Hansen</pre>
        /// 
        ///     The above code might be difficult to understand when you start out with p5.lambda. However, to explain it, it basically passes in
        ///     the [_result] node by reference to the p5.lambda block executed by the [lambda] statement. The [lambda] block itself, will be 
        ///     created from the string, which is the value of the above [lambda] node, converted into a p5.lambda node-set. In addition it 
        ///     passes in the [_input] parameter. Since we are executing our p5.lambda block, which is converted from text above, the only way 
        ///     to return values from that [lambda] block, is to pass in nodes by reference, for then to have the [lambda] block modify the 
        ///     contents of that node, and/or node hierarchy.
        /// 
        ///     A highly useful example of why you'd like to use constructs such as the above, is to realize you can use the above logic to
        ///     load p5.lambda files, for then to pass in arguments to the execution of those p5.lambda files. Consider the following code;
        /// 
        ///     <pre>load-file:some-hyperlisp-file.hl
        /// lambda:@/./0?value
        ///   some-input-parameter:foo
        ///   some-return-value:node:</pre>
        /// 
        ///     The above construct allows you to perceive files as <em>"functions"</em>, and pass in and return arguments to and from these files.
        ///     This is a pattern often used when you use p5.lambda and Phosphorus Five. In fact, that's where the name p5.lambda comes from, since
        ///     in p5.lambda, everything is a potential executable piece of <em>"code"</em>.
        /// 
        ///     Another highly useful trait of constructs such as the above, is that you can pass code across HTTP requests, and boundaries where
        ///     execution-trees normally don't travel easily, which facilitates for passing pieces of code from one server on your intranet, to
        ///     another server, which creates better scaling-out capabilities for your end solutions.
        /// </summary>
        /// <param name="context">Application context.</param>
        /// <param name="e">Parameters passed into Active Event.</param>
        [ActiveEvent (Name = "lambda")]
        private static void lambda_lambda (ApplicationContext context, ActiveEventArgs e)
        {
            Executor (ExecuteBlockNormal, context, e.Args, "lambda");
        }

        /// <summary>
        ///     Executes a specified piece of p5.lambda block.
        /// 
        ///     The [lambda-immutable] keyword, works similar to the [lambda] keyword, except that whatever code you start out with, will also
        ///     be the code you end up with, since [lambda-immutable] will set the code block it executes back, to whatever it contained before
        ///     execution. This allows you to create code-blocks that after execution will be apparently unchanged. Consider the following code;
        /// 
        ///     <pre>
        /// lambda-immutable
        ///   _foo
        ///   set:@/./0?value
        ///     src:Howdy World</pre>
        /// 
        ///     If you change the above [lambda-immutable] statement, to become a [lambda] statement, then the value of [_foo] will become 
        ///     <em>"Hello World"</em> after execution. But since we're using [lambda-immutable], then the code block executed, will be set back
        ///     to its original state, after execution.
        /// 
        ///     Besides from that, [lambda-immutable] is identical to [lambda]. See the <see cref="p5.lambda.Lambda.lambda_lambda">[lambda]</see>
        ///     keyword's documentation, to understand what more features you can use with [lambda-immutable].
        /// </summary>
        /// <param name="context">Application context.</param>
        /// <param name="e">Parameters passed into Active Event.</param>
        [ActiveEvent (Name = "lambda-immutable")]
        private static void lambda_immutable (ApplicationContext context, ActiveEventArgs e)
        {
            Executor (ExecuteBlockImmutable, context, e.Args, "lambda-immutable");
        }

        /// <summary>
        ///     Executes a specified piece of p5.lambda block.
        /// 
        ///     The [lambda-copy] keyword, works similar to the [lambda] keyword, except that [lambda-copy] will create a copy of whatever p5.lambda
        ///     execution blocks you executes, with the execution blocks becoming the root nodes of you [lambda-copy] statement. Consider the 
        ///     following code;
        /// 
        ///     <pre>_out:error
        /// _exe
        ///   _out:success
        ///   set:@/./"*"/_input/#?name
        ///     source:@/../"*"/_out?value
        /// lambda-copy:@/-?node
        ///   _input:node:</pre>
        /// 
        ///     If you change the above [lambda-copy] statement, to become a [lambda] statement, then the name of the [_input] reference node 
        ///     will become <em>"error"</em> after execution. But since we're using [lambda-copy], then the root node iterator, in the [source]
        ///     parameter above, will actually access the [_exe] node, and not the root node for the entire execution tree, and hence the [_input]'s
        ///     reference node's name will be set to <em>"success"</em> and not <em>"error"</em>.
        /// 
        ///     Another thing you'll notice, is that since we're executing the [lambda-copy] on a copy of the executedd nodes, then the execution
        ///     tree will not be modified, the same way it is not modified when using [lambda-immutable], though for different reasons obviously.
        /// 
        ///     Besides from that, [lambda-copy] is identical to [lambda]. See the <see cref="p5.lambda.Lambda.lambda_lambda">[lambda]</see>
        ///     keyword's documentation, to understand what more features you can use with [lambda-immutable].
        /// </summary>
        /// <param name="context">Application context.</param>
        /// <param name="e">Parameters passed into Active Event.</param>
        [ActiveEvent (Name = "lambda-copy")]
        private static void lambda_copy (ApplicationContext context, ActiveEventArgs e)
        {
            Executor (ExecuteBlockCopy, context, e.Args, "lambda-copy");
        }

        /// <summary>
        ///     Executes a specified piece of p5.lambda statement.
        /// 
        ///     The [lambda-single] keyword, works similar to the [lambda] keyword, except that [lambda-single] will execute one single statement,
        ///     and not a block of code. Consider the following code, and notice how we're not executing the block, but the actual [set] node as 
        ///     a <em>"single"</em> statement;
        /// 
        ///     <pre>_exe
        ///   set:\@/.?value
        ///     source:Hello World
        /// lambda-single:\@/-/0?node</pre>
        /// 
        /// </summary>
        /// <param name="context">Application context.</param>
        /// <param name="e">Parameters passed into Active Event.</param>
        [ActiveEvent (Name = "lambda-single")]
        private static void lambda_single (ApplicationContext context, ActiveEventArgs e)
        {
            if (e.Args.Value == null)
                throw new LambdaException ("nothing was given to [lambda-single] for execution", e.Args, context);

            // executing a value object, converting to node, before we pass into execution method,
            // making sure we pass in children of [lambda] as "arguments" or "parameters" to [lambda] statement
            var match = Expression.Create (e.Args.Get<string> (context), context).Evaluate (e.Args, context);
            foreach (var idxSource in match) {
                ExecuteStatement (Utilities.Convert<Node> (idxSource.Value, context), context, true);
            }
        }

        /*
         * executes a block of nodes, this is where the actual execution happens
         * this is the "heart beat" method of the "p5.lambda" execution engine
         */
        private static void ExecuteBlockNormal (ApplicationContext context, Node exe, IEnumerable<Node> args)
        {
            // passing in arguments, if there are any
            foreach (var idx in args) {
                exe.Add (idx.Clone ());
            }

            // iterating through all nodes in execution scope, and raising these as Active Events
            var idxExe = exe.FirstChild;
            while (idxExe != null) {
                // executing current statement and retrieving next execution statement
                idxExe = ExecuteStatement (idxExe, context, false);
            }
        }

        /*
         * executes a block of nodes, this is where the actual execution happens
         * this is the "heart beat" method of the "p5.lambda" execution engine
         */
        private static void ExecuteBlockImmutable (ApplicationContext context, Node exe, IEnumerable<Node> args)
        {
            // storing original execution nodes, such that we can set back execution
            // block to what it originally was
            var oldNodes = exe.Children.Select (idx => idx.Clone ()).ToList ();

            // passing in arguments, if there are any
            foreach (var idx in args) {
                exe.Add (idx.Clone ());
            }

            // iterating through all nodes in execution scope, and raising these as Active Events
            var idxExe = exe.FirstChild;
            while (idxExe != null) {
                // executing current statement and retrieving next execution statement
                idxExe = ExecuteStatement (idxExe, context);
            }

            // making sure we set back execution block to original nodes
            exe.Clear ();
            exe.AddRange (oldNodes);
        }

        /*
         * executes a block of nodes, this is where the actual execution happens
         * this is the "heart beat" method of the "p5.lambda" execution engine
         */
        private static void ExecuteBlockCopy (ApplicationContext context, Node exe, IEnumerable<Node> args)
        {
            // making sure lambda is executed on copy of execution nodes, if we should,
            // without access to nodes outside of its own scope
            exe = exe.Clone ();

            // passing in arguments, if there are any
            foreach (var idx in args) {
                exe.Add (idx.Clone ());
            }

            // iterating through all nodes in execution scope, and raising these as Active Events
            var idxExe = exe.FirstChild;
            while (idxExe != null) {

                // executing current statement and retrieving next execution statement
                idxExe = ExecuteStatement (idxExe, context);
            }
        }

        /*
         * executes one execution statement
         */
        private static Node ExecuteStatement (Node exe, ApplicationContext context, bool force = false)
        {
            // storing "next execution node" as fallback, to support "delete this node" pattern
            var nextFallback = exe.NextSibling;

            // we don't execute nodes that start with an underscore "_" since these are considered "data segments"
            // also we don't execute nodes with no name, since these interfers with "null Active Event handlers"
            if (force || (!exe.Name.StartsWith ("_") && exe.Name != string.Empty)) {
                // raising the given Active Event normally, taking inheritance chain into account
                context.Raise (exe.Name, exe);
            }

            // prioritizing "NextSibling", in case this node created new nodes, while having
            // nextFallback as "fallback node", in case current execution node removed current execution node,
            // but if "current execution node" untied nextFallback, in addition to "NextSibling",
            // we return null back to caller
            return exe.NextSibling ?? (nextFallback != null && nextFallback.Parent != null &&
                                       nextFallback.Parent == exe.Parent ? nextFallback : null);
        }
    }
}
