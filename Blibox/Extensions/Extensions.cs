using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Blibox
{
    public static class Extensions
    {

        /// <summary>
        /// Used to determine the direction of the sort identifier used when filtering lists
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="sortOrder">the current sort order being used on the page</param>
        /// <param name="field">the field that we are attaching this sort identifier to</param>
        /// <returns>MvcHtmlString used to indicate the sort order of the field</returns>
        public static IHtmlString SortIdentifier(this HtmlHelper htmlHelper, string sortOrder, string field)
        {
            if (string.IsNullOrEmpty(sortOrder) || (sortOrder.Trim() != field && sortOrder.Replace("_desc", "").Trim() != field)) return null;

            string glyph = "glyphicon glyphicon-chevron-up";
            if (sortOrder.ToLower().Contains("desc"))
            {
                glyph = "glyphicon glyphicon-chevron-down";
            }

            var span = new TagBuilder("span");
            span.Attributes["class"] = glyph;

            return MvcHtmlString.Create(span.ToString());
        }

        /// <summary>
        /// Converts a NameValueCollection into a RouteValueDictionary containing all of the elements in the collection, and optionally appends
        /// {newKey: newValue} if they are not null
        /// </summary>
        /// <param name="collection">NameValue collection to convert into a RouteValueDictionary</param>
        /// <param name="newKey">the name of a key to add to the RouteValueDictionary</param>
        /// <param name="newValue">the value associated with newKey to add to the RouteValueDictionary</param>
        /// <returns>A RouteValueDictionary containing all of the keys in collection, as well as {newKey: newValue} if they are not null</returns>
        public static RouteValueDictionary ToRouteValueDictionary(this NameValueCollection collection, string newKey, string newValue)
        {
            var routeValueDictionary = new RouteValueDictionary();
            foreach (var key in collection.AllKeys)
            {
                if (key == null) continue;
                if (routeValueDictionary.ContainsKey(key))
                    routeValueDictionary.Remove(key);

                routeValueDictionary.Add(key, collection[key]);
            }
            if (string.IsNullOrEmpty(newValue))
            {
                routeValueDictionary.Remove(newKey);
            }
            else
            {
                if (routeValueDictionary.ContainsKey(newKey))
                    routeValueDictionary.Remove(newKey);

                routeValueDictionary.Add(newKey, newValue);
            }
            return routeValueDictionary;
        }

        public static MvcHtmlString AddInputPrefix(this MvcHtmlString htmlString, string prefix)
        {
            // Estos son los nuevos formatos para los valores de los atributos "id" y "name"
            // de los inputs cuando tienen que ser colecciones o arreglos,
            // La idea es que el formato para los ids sea Propiedad_Indice_SubPropiedad y
            // que el formato para los names sea Propiedad[Indice].SubPropiedad.

            var newIdFormat = "$1=\"" + prefix + "_${index}_$2\"";
            var newNameFormat = "name=\"" + prefix + "[${index}].$1\"";

            var regexOptions = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled;

            // idAttributeRegex para buscar textos que sean id="algo" o data-valmsg-for="algo",
            // nameAttributeRegex para buscar textos que sean name="algo".
            var idAttributeRegex = new Regex("(id|data-valmsg-for)=\"(.+?)\"", regexOptions);
            var nameAttributeRegex = new Regex("name=\"(.+?)\"", regexOptions);

            // Se remplazan los ids y names originales por el nuevo formato deseado.
            var html = htmlString.ToString();
            html = idAttributeRegex.Replace(html, newIdFormat);
            html = nameAttributeRegex.Replace(html, newNameFormat);

            // Si existe el atributo name="Propiedad[Indice].Index" se remplaza por el atributo
            // name="Propiedad[].Index"
            html = html.Replace("name=\"" + prefix + "[${index}].Index\"", "name=\"" + prefix + "[].Index\"");

            return new MvcHtmlString(html);
        }
    }
}