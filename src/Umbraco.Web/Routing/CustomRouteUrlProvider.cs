﻿using System;
using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;

namespace Umbraco.Web.Routing
{
    /// <summary>
    /// This url provider is used purely to deal with umbraco custom routes that utilize UmbracoVirtualNodeRouteHandler and will return
    /// the URL returned from the current PublishedContentRequest.PublishedContent (virtual node) if the request is in fact a virtual route and
    /// the id that is being requested matches the id of the current PublishedContentRequest.PublishedContent.
    /// </summary>
    internal class CustomRouteUrlProvider : IUrlProvider
    {
        /// <summary>
        /// This will simply return the URL that is returned by the assigned IPublishedContent if this is a custom route
        /// </summary>
        public string GetUrl(UmbracoContext umbracoContext, IPublishedContent content, UrlProviderMode mode, string culture, Uri current)
        {
            if (umbracoContext?.PublishedRequest?.PublishedContent == null) return null;
            if (umbracoContext.HttpContext?.Request?.RequestContext?.RouteData?.DataTokens == null) return null;
            if (umbracoContext.HttpContext.Request.RequestContext.RouteData.DataTokens.ContainsKey(Core.Constants.Web.CustomRouteDataToken) == false) return null;

            //ok so it's a custom route with published content assigned, check if the id being requested for is the same id as the assigned published content
            return content.Id == umbracoContext.PublishedRequest.PublishedContent.Id
                ? umbracoContext.PublishedRequest.PublishedContent.GetUrl(culture) // fixme ∞ loop.
                : null;
        }

        /// <summary>
        /// This always returns null because this url provider is used purely to deal with Umbraco custom routes with
        /// UmbracoVirtualNodeRouteHandler, we really only care about the normal URL so that RedirectToCurrentUmbracoPage() works
        /// with SurfaceControllers
        /// </summary>
        /// <param name="umbracoContext"></param>
        /// <param name="id"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public IEnumerable<string> GetOtherUrls(UmbracoContext umbracoContext, int id, Uri current)
        {
            return null;
        }
    }
}
