
namespace HulkEngine
{
    public class NodeVisitor
    {
        // Visit generates the visit to the referenced node by concatenating 
        // "Visit_" with the type of object with which the Visit method was invoked.
        public object Visit(dynamic node)
        {
            string method_name = "Visit_" + node.GetType().Name;
            var visitor = GetType().GetMethod(method_name);
            return visitor?.Invoke(this, new object[] {node}) ?? GenericVisit(node);
        }

        public void GenericVisit(dynamic node)
        {
            throw new Exception($"No Visit_{GetType().Name} method");
        }
    }
}
