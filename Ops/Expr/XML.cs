using System.Xml.Linq;
namespace PSI;

// A basic XML generator, implemented using the Visitor pattern
public class ExprXML : Visitor<XElement> {
   public override XElement Visit (NLiteral literal) {
      var node = new XElement ("Literal");
      node.SetAttributeValue ("Value", literal.Value);
      node.SetAttributeValue ("Type", literal.Type);
      return node;
   }

   public override XElement Visit (NIdentifier identifier) {
      var node = new XElement ("Ident");
      node.SetAttributeValue ("Name", identifier.Name);
      node.SetAttributeValue ("Type", identifier.Type);
      return node;
   }

   public override XElement Visit (NUnary unary) {
      var node = new XElement ("Unary");
      node.Add (unary.Accept (this));
      node.SetAttributeValue ("Op", unary.Op.Kind);
      node.SetAttributeValue ("Type", unary.Type);
      return node;
   }

   public override XElement Visit (NBinary binary) {
      var node = new XElement ("Binary");
      node.Add (binary.Left.Accept (this));
      node.Add (binary.Right.Accept (this));
      node.SetAttributeValue ("Op", binary.Op.Kind);
      node.SetAttributeValue ("Type", binary.Type);
      return node;
   }
}