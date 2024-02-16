using System.Xml.Linq;
namespace PSI;

// A basic XML generator, implemented using the Visitor pattern
public class ExprXML : Visitor<XElement> {
   public override XElement Visit (NLiteral literal)
      => Make ("Literal", ("Value", literal.Value), ("Type", literal.Type));

   public override XElement Visit (NIdentifier identifier)
      => Make ("Ident", ("Name", identifier.Name), ("Type", identifier.Type));

   public override XElement Visit (NUnary unary)
      => Make ("Unary",
         unary.Accept (this),
         ("Op", unary.Op.Kind), ("Type", unary.Type));

   public override XElement Visit (NBinary binary)
      => Make ("Binary",
         binary.Left.Accept (this), binary.Right.Accept (this),
         ("Op", binary.Op.Kind), ("Type", binary.Type));

   XElement Make (string name, params object[] a) {
      var node = new XElement (name);
      foreach (var value in a)
         switch (value) {
            case XElement xe: node.Add (xe); break;
            case (string Name, object Value): node.SetAttributeValue (Name, Value); break;
            default: node.SetValue (value); break;
         }
      return node;
   }
}