using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Framework.Contexts;
using Microsoft.AspNet.SignalR;
using MyAndromeda.Logging;

namespace MyAndromeda.SignalRHubs
{

    public static class HubGroupExtensions 
    {
        internal const string StoreGroupFormat = "MyAndreomda.Store.Groups.{0}";
        internal const string ChainGroupFormat = "MyAndromeda.Chain.Groups.{0}";
        internal const string RoleGroupFromat = "MyAndromeda.Role.Groups.{0}";

        public static string GetStoreGroup(this IHubContext context, int andromedaSiteId) 
        {
            return string.Format(StoreGroupFormat, andromedaSiteId);
        }

        public static string GetRoleGroup(this IHubContext context, string roleName) 
        {
            return string.Format(RoleGroupFromat, roleName);
        }

        public static IEnumerable<string> GetUserConnections(this IHubContext context, int userId) 
        {
            List<string> connections = null;
            if (HubExtensions.UserConnections.TryGetValue(userId, out connections)) 
            {
                return connections.Select(e=> e);
            }

            return Enumerable.Empty<string>();
        }
    }

    public static class HubExtensions
    {
        public static ConcurrentDictionary<int, List<string>> UserConnections = new ConcurrentDictionary<int, List<string>>(); 
        public static ConcurrentDictionary<int, List<int>> ChainUsers = new ConcurrentDictionary<int,List<int>>();
        public static ConcurrentDictionary<int, List<int>> StoreUsers = new ConcurrentDictionary<int, List<int>>();
        public static ConcurrentDictionary<string, List<int>> RolesAndUsers = new ConcurrentDictionary<string,List<int>>();

        //private static IMyAndromedaLogger GetLogger() { return System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IMyAndromedaLogger)) as IMyAndromedaLogger; }

        /// <summary>
        /// Adds a user to a group 
        /// </summary>
        /// <param name="hub">The hub.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public static void MyAndromedaJoin(this Hub hub, ICurrentUser user, ICurrentSite site) 
        {
            JoinProcess(hub, user, site);
        }

        public static void MyAndromedaReJoin(this Hub hub, ICurrentUser user, ICurrentSite site) 
        {
            MyAndromedaLeave(hub, user);
            JoinProcess(hub, user, site);
        }

        private static void JoinProcess(this Hub hub, ICurrentUser user, ICurrentSite site) 
        {
            AddSelfToUserConnectionList(user, hub);
            AddSelfToChainList(user, hub);
            AddSelfToStoreList(user, site, hub);
            AddSelfToRoleList(user, hub);
        }

        public static void MyAndromedaLeave(this Hub hub, ICurrentUser user) 
        {
            RemoveSelfFromConnectionList(user, hub);
            RemoveSelfFromChainList(user, hub);
            RemoveSelfFromRoleList(user, hub);
        }

        private static void AddSelfToUserConnectionList(ICurrentUser user, Hub hub) 
        {
            if (!user.Available) { return; }

            var group = UserConnections.GetOrAdd(user.User.Id, (id) => new List<string>());

            if (!group.Contains(hub.Context.ConnectionId))
            {
                group.Add(hub.Context.ConnectionId);
            }
        }

        private static void RemoveSelfFromConnectionList(ICurrentUser user, Hub hub) 
        {
            if (!user.Available) { return; }

            if (!UserConnections.ContainsKey(user.User.Id))
                return;

            var userHasTheseConnections = UserConnections.GetOrAdd(user.User.Id, id => new List<string>());

            if (userHasTheseConnections.Count == 0)
            {
                List<string> removedGroup = null;
                UserConnections.TryRemove(user.User.Id, out removedGroup);
            }
            else 
            {
                lock (userHasTheseConnections)
                { 
                    if (userHasTheseConnections.Contains(hub.Context.ConnectionId)) 
                    { 
                        userHasTheseConnections.Remove(hub.Context.ConnectionId);
                    }
                }
            }
        }

        private static void AddSelfToChainList(ICurrentUser user, Hub hub)
        {
            if (!user.Available)
            {
                return;
            }

            foreach (var chain in user.FlattenedChains) 
            {
                var group = ChainUsers.GetOrAdd(chain.Id, new List<int>());

                if (group.Contains(user.User.Id)) 
                {
                    continue;
                }

                lock (group)
                { 
                    group.Add(user.User.Id);
                }

                hub.Groups.Add(hub.Context.ConnectionId, string.Format(HubGroupExtensions.ChainGroupFormat, chain.Id));
            }
        }

        private static void AddSelfToStoreList(ICurrentUser user, ICurrentSite site, Hub hub)
        {
            if (!user.Available)
                return;
            if (!site.Available)
                return;

            var group = StoreUsers.GetOrAdd(site.AndromediaSiteId, new List<int>());

            if (!group.Contains(user.User.Id)) 
            {
                lock (group) 
                { 
                    group.Add(user.User.Id);
                }
            }

            hub.Groups.Add(hub.Context.ConnectionId, string.Format(HubGroupExtensions.StoreGroupFormat, site.AndromediaSiteId));
        }

        private static void RemoveSelfFromChainList(ICurrentUser user, Hub hub) 
        {
            foreach (var chain in user.FlattenedChains) 
            {
                var group = ChainUsers.GetOrAdd(chain.Id, new List<int>());

                if (!group.Contains(user.User.Id)) { continue; }

                lock (group) { 
                    group.Remove(user.User.Id);
                }

                string groupName = string.Format(HubGroupExtensions.ChainGroupFormat, chain.Id);
                hub.Groups.Remove(hub.Context.ConnectionId, groupName);
            }
        }

        private static void AddSelfToRoleList(ICurrentUser user, Hub hub) 
        {
            if (!user.Available)
                return;

            foreach (var role in user.Roles) 
            {
                var group = RolesAndUsers.GetOrAdd(role.Name, new List<int>());

                hub.Groups.Add(hub.Context.ConnectionId, string.Format(HubGroupExtensions.RoleGroupFromat, role.Name));

                if (group.Contains(user.User.Id)) { continue; }

                lock (group) { 
                    group.Add(user.User.Id);
                }
            }
        }

        private static void RemoveSelfFromRoleList(ICurrentUser user, Hub hub) 
        {
            foreach (var role in user.Roles) 
            {
                var group = RolesAndUsers.GetOrAdd(role.Name, new List<int>());

                if (!group.Contains(user.User.Id)) { continue; }

                lock (group)  {
                    group.Add(user.User.Id);
                }

                string groupName = string.Format(HubGroupExtensions.RoleGroupFromat, role.Name);
                hub.Groups.Remove(hub.Context.ConnectionId, groupName);

                hub.Clients.Group(groupName).left(user.User.Username);
            }
        }
    }
}
