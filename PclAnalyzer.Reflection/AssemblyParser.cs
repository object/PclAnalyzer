using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;
using PclAnalyzer.Core;

namespace PclAnalyzer.Reflection
{
    public class AssemblyParser
    {
        private readonly string _assemblyPath;

        public AssemblyParser(string assemblyPath)
        {
            _assemblyPath = assemblyPath;
        }

        public IList<MethodCall> GetAssemblyCalls()
        {
            var module = ModuleDefinition.ReadModule(_assemblyPath);

            var methods = from t in module.Types
                          from m in t.Methods
                          where m.Body != null
                          select m;

            return GetMethodCalls(module, methods).ToList();
        }

        public IList<MethodCall> GetTypeCalls(string typeName)
        {
            var module = ModuleDefinition.ReadModule(_assemblyPath);

            var methods = from t in module.Types
                          where GetTypeFullName(t) == typeName
                          from m in t.Methods
                          where m.Body != null
                          select m;

            return GetMethodCalls(module, methods).ToList();
        }

        public IList<MethodCall> GetMethodCalls(string typeName, string methodName)
        {
            var module = ModuleDefinition.ReadModule(_assemblyPath);

            var method = (from t in module.Types
                          where GetTypeFullName(t) == typeName
                          from m in t.Methods
                          where m.Body != null && GetMethodFullName(m) == string.Join(".", typeName, methodName)
                          select m).Single();

            return GetMethodCalls(module, new[] { method }).ToList();
        }

        private IEnumerable<MethodCall> GetMethodCalls(ModuleDefinition module, IEnumerable<MethodDefinition> methods)
        {
            var methodCalls = from m in methods
                              from i in m.Body.Instructions
                              where IsExternalCall(module, i)
                              select new MethodCall(GetMethodInfo(m), GetMethodInfo(i.Operand as MethodReference));

            return methodCalls.Distinct().Where(x => !IsExcluded(x.ReferencedMethod)).ToList();
        }

        private string GetTypeFullName(TypeDefinition type)
        {
            return string.Join(".", type.Namespace, type.Name);
        }

        private string GetMethodFullName(MethodDefinition method)
        {
            return string.Join(".", method.DeclaringType.Namespace, method.DeclaringType.Name, method.Name);
        }

        private bool IsExternalCall(ModuleDefinition module, Instruction i)
        {
            return i.Operand is MethodReference && (i.Operand as MethodReference).DeclaringType.Scope != module;
        }

        private bool IsExcluded(Member methodInfo)
        {
            return methodInfo.TypeName.Contains("[") || methodInfo.TypeName.Contains("]");
        }

        private Member GetMethodInfo(MethodDefinition method)
        {
            return new Member(method.DeclaringType.Scope.Name, method.DeclaringType.Namespace, method.DeclaringType.Name, method.Name);
        }

        private Member GetMethodInfo(MethodReference method)
        {
            return new Member(method.DeclaringType.Scope.Name, method.DeclaringType.Namespace, method.DeclaringType.Name, method.Name);
        }
    }
}
