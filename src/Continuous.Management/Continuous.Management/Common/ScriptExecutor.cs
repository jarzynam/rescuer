﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;

namespace Continuous.Management.Common
{
    internal interface IScriptExecutor
    {
        ICollection<PSObject> Execute(string scriptFullPath, ICollection<CommandParameter> parameters);
    }

    internal class ScriptExecutor : IScriptExecutor
    {
        private readonly IEmbededFileReader _embededFileReader;
        
        public ScriptExecutor(Type typeForAssembly)
        {
            _embededFileReader = new EmbededFileReader(typeForAssembly);
        }

        public ICollection<PSObject> Execute(string scriptFullPath, ICollection<CommandParameter> parameters)
        {
            var script = _embededFileReader.Read(scriptFullPath);

            using (var runspace = RunspaceFactory.CreateRunspace())
            {
                runspace.Open();

                using (var pipeline = runspace.CreatePipeline())
                {
                    var cmd = CreateCommand(parameters, script);

                    pipeline.Commands.Add(cmd);

                    var results = pipeline.Invoke();

                    ThrowErrorIfNecessary(pipeline);

                    return results;
                }
            }
        }

        private static Command CreateCommand(ICollection<CommandParameter> parameters, string script)
        {
            var cmd = new Command(script, true);

            if (parameters == null) return cmd;

            foreach (var p in parameters)
                cmd.Parameters.Add(p);
            
            return cmd;
        }


        private void ThrowErrorIfNecessary(Pipeline pipeline)
        {
            if (!pipeline.HadErrors) return;

            var errors = pipeline.Error.Read() as Collection<ErrorRecord>;

            if (errors == null || errors.Count == 0) return;

            var errorBuilder = new StringBuilder();

            foreach (var error in errors)
                errorBuilder.AppendLine(error.Exception.Message);

            throw new InvalidOperationException(errorBuilder.ToString());
        }
    }
}