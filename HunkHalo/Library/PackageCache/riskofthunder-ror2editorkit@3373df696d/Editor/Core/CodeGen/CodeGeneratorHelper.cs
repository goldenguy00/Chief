using Microsoft.CSharp;
using System;
using System.Text;

namespace RoR2.Editor.CodeGen
{
    /// <summary>
    /// The CodeGeneratorHelper is a Disposable class that contains utility methods for generating code.
    /// 
    /// <br>Contains methods for simplifying the creation of unique identifiers and more.</br>
    /// <para>This class must be disposed of properly once its usage is finished, utilization of a "using()" clause is recommended.</para>
    /// </summary>
    public class CodeGeneratorHelper : IDisposable
    {
        private CSharpCodeProvider _codeProvider;

        /// <summary>
        /// Generates a Code Header that informs the user that a code was auto generated. by writing a large comment block with information, such as the name of the tool that generated the code, the version of the tool, and optionally a file name from which the code was generated
        /// </summary>
        /// <param name="toolName">The name of the tool that generated this code</param>
        /// <param name="toolVersion">The version of the tool</param>
        /// <param name="sourceFileName">Optional file name that contains the data used to write the code</param>
        /// <returns>A large string containing the header, write it using <see cref="Writer.WriteLine()"/></returns>
        public string MakeAutoGeneratedCodeHeader(string toolName, string toolVersion, string sourceFileName = null)
        {
            return
                "//------------------------------------------------------------------------------\n"
                + "// <auto-generated>\n"
                + $"//     This code was auto-generated by {toolName}\n"
                + $"//     version {toolVersion}\n"
                + (string.IsNullOrEmpty(sourceFileName) ? "" : $"//     from {sourceFileName}\n")
                + "//\n"
                + "//     Changes to this file may cause incorrect behavior and will be lost if\n"
                + "//     the code is regenerated.\n"
                + "// </auto-generated>\n"
                + "//------------------------------------------------------------------------------\n";
        }

        /// <summary>
        /// Creates a "CONSTANT" style identifier for the given string
        /// </summary>
        /// <param name="identifier">The identifier to transform into a CONSTANT</param>
        /// <returns>The constant identifier, example: "my Constant" gets transformed into "MY_CONSTANT"</returns>
        public string MakeIdentifierConstant(string identifier)
        {
            var pascalCase = MakeIdentifierPascalCase(identifier);
            char[] chars = pascalCase.ToCharArray();
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < chars.Length; i++)
            {
                var chr = chars[i];

                if (char.IsUpper(chr))
                {
                    if (i != 0)
                    {
                        builder.Append("_");
                    }
                    builder.Append(chr);
                }
                else if (char.IsLower(chr))
                {
                    builder.Append(char.ToUpperInvariant(chr));
                }
                else
                {
                    builder.Append(chr);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Creates a "PascalCase" style identifier for the given string
        /// </summary>
        /// <param name="identifier">The identifier to transform into PascalCase</param>
        /// <returns>The PascalCase identifier, example: "My pascal case" gets transformed into "MyPascalCase"</returns>
        public string MakeIdentifierPascalCase(string identifier)
        {
            return _codeProvider.CreateValidIdentifier(identifier).Replace(" ", "");
        }


        /// <summary>
        /// Creates a "camelCase" style identifier for the given string
        /// </summary>
        /// <param name="identifier">The identifier to transform into camelCase</param>
        /// <returns>The camelCase identifier, example: "My camel case" gets transformed into "myCamelCase"</returns>
        public string MakeIdentifierCamelCase(string identifier)
        {
            string camelCase = MakeIdentifierPascalCase(identifier);
            char[] chars = camelCase.ToCharArray();
            chars[0] = char.ToLowerInvariant(chars[0]);
            return new string(chars);
        }

        /// <summary>
        /// Disposes the code generator helper.
        /// </summary>
        public void Dispose()
        {
            _codeProvider?.Dispose();
        }

        /// <summary>
        /// Constructor for CodeGeneratorHelper
        /// </summary>
        public CodeGeneratorHelper()
        {
            _codeProvider = new CSharpCodeProvider();
        }
    }
}