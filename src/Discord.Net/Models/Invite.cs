﻿using Discord.API.Client;
using System;
using APIInvite = Discord.API.Client.Invite;

namespace Discord
{
	public sealed class Invite
	{
		public sealed class ServerInfo
		{
			/// <summary> Returns the unique identifier of this server. </summary>
			public ulong Id { get; }
			/// <summary> Returns the name of this server. </summary>
			public string Name { get; }

			internal ServerInfo(ulong id, string name)
			{
				Id = id;
				Name = name;
			}
		}
		public sealed class ChannelInfo
		{
			/// <summary> Returns the unique identifier of this channel. </summary>
			public ulong Id { get; }
			/// <summary> Returns the name of this channel. </summary>
			public string Name { get; }

			internal ChannelInfo(ulong id, string name)
			{
				Id = id;
				Name = name;
			}
		}
		public sealed class InviterInfo
		{
			/// <summary> Returns the unique identifier for this user. </summary>
			public ulong Id { get; }
			/// <summary> Returns the name of this user. </summary>
			public string Name { get; }
			/// <summary> Returns the by-name unique identifier for this user. </summary>
			public ushort Discriminator { get; }
			/// <summary> Returns the unique identifier for this user's avatar. </summary>
			public string AvatarId { get; }
			/// <summary> Returns the full path to this user's avatar. </summary>
			public string AvatarUrl => User.GetAvatarUrl(Id, AvatarId);

			internal InviterInfo(ulong id, string name, ushort discriminator, string avatarId)
			{
				Id = id;
				Name = name;
				Discriminator = discriminator;
				AvatarId = avatarId;
			}
		}

		/// <summary> Returns information about the server this invite is attached to. </summary>
		public ServerInfo Server { get; private set; }
		/// <summary> Returns information about the channel this invite is attached to. </summary>
		public ChannelInfo Channel { get; private set; }
        
        public string Code { get; }
        /// <summary> Returns, if enabled, an alternative human-readable code for URLs. </summary>
        public string XkcdCode { get; }
		/// <summary> Time (in seconds) until the invite expires. Set to 0 to never expire. </summary>
		public int MaxAge { get; private set; }
		/// <summary> The amount  of times this invite has been used. </summary>
		public int Uses { get; private set; }
		/// <summary> The max amount  of times this invite may be used. </summary>
		public int MaxUses { get; private set; }
		/// <summary> Returns true if this invite has been destroyed, or you are banned from that server. </summary>
		public bool IsRevoked { get; private set; }
		/// <summary> If true, a user accepting this invite will be kicked from the server after closing their client. </summary>
		public bool IsTemporary { get; private set; }
		public DateTime CreatedAt { get; private set; }

		/// <summary> Returns a URL for this invite using XkcdCode if available or Id if not. </summary>
		public string Url => $"{DiscordConfig.InviteUrl}/{Code}";

        internal Invite(string code, string xkcdPass)
		{
            Code = code;
			XkcdCode = xkcdPass;
		}

		internal void Update(InviteReference model)
		{
			if (model.Guild != null)
				Server = new ServerInfo(model.Guild.Id, model.Guild.Name);
			if (model.Channel != null)
				Channel = new ChannelInfo(model.Channel.Id, model.Channel.Name);
        }
        internal void Update(APIInvite model)
		{
			Update(model as InviteReference);

			if (model.IsRevoked != null)
				IsRevoked = model.IsRevoked.Value;
			if (model.IsTemporary != null)
				IsTemporary = model.IsTemporary.Value;
			if (model.MaxAge != null)
				MaxAge = model.MaxAge.Value;
			if (model.MaxUses != null)
				MaxUses = model.MaxUses.Value;
			if (model.Uses != null)
				Uses = model.Uses.Value;
			if (model.CreatedAt != null)
				CreatedAt = model.CreatedAt.Value;
		}

		public override bool Equals(object obj) => obj is Invite && (obj as Invite).Code == Code;
		public override int GetHashCode() => unchecked(Code.GetHashCode() + 9980);
		public override string ToString() => XkcdCode ?? Code;
	}
}
