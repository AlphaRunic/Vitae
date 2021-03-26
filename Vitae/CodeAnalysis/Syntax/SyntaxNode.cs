using System.Collections.Generic;
using System.Reflection;

namespace Vitae.CodeAnalysis.Syntax
{
    public abstract class SyntaxNode
    {
        public abstract SyntaxType Type { get; }

        public IEnumerable<SyntaxNode> GetChildren()
        {
            PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (var property in properties)
            {
                if (typeof(SyntaxNode).IsAssignableFrom(property.PropertyType))
                {
                    var child = (SyntaxNode) property.GetValue(this);
                    yield return child;
                }
                else if (typeof(IEnumerable<SyntaxNode>).IsAssignableFrom(property.PropertyType))
                {
                    var children = (IEnumerable<SyntaxNode>) property.GetValue(this);
                    foreach (var child in children)
                        yield return child;
                }
            }
        }
    }
}