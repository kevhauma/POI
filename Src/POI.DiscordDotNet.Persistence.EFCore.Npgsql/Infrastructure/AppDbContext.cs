﻿using Microsoft.EntityFrameworkCore;
using POI.DiscordDotNet.Persistence.Domain;

namespace POI.DiscordDotNet.Persistence.EFCore.Npgsql.Infrastructure;

internal class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	public DbSet<ServerSettings> ServerSettings { get; set; }

	public DbSet<GlobalUserSettings> GlobalUserSettings { get; set; }
	public DbSet<ServerDependentUserSettings> ServerDependentUserSettings { get; set; }
	public DbSet<AccountLinks> AccountLinks { get; set; }

	public DbSet<LeaderboardEntry> LeaderboardEntries { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		var serverSettingsModelBuilder = modelBuilder.Entity<ServerSettings>();
		serverSettingsModelBuilder.HasKey(x => x.ServerId);

		var globalUserSettingsModelBuilder = modelBuilder.Entity<GlobalUserSettings>();
		globalUserSettingsModelBuilder.HasKey(x => x.UserId);

		var serverDependentUserSettingsModelBuilder = modelBuilder.Entity<ServerDependentUserSettings>();
		serverDependentUserSettingsModelBuilder.HasKey(x => new {x.UserId, x.ServerId});

		var accountLinksModelBuilder = modelBuilder.Entity<AccountLinks>();
		accountLinksModelBuilder.HasKey(x => x.DiscordId);
		accountLinksModelBuilder.Property(x => x.ScoreSaberId).IsRequired(false).HasMaxLength(20);
		accountLinksModelBuilder.HasIndex(x => x.ScoreSaberId).IsUnique();

		var leaderboardEntryModelBuilder = modelBuilder.Entity<LeaderboardEntry>();
		leaderboardEntryModelBuilder.HasKey(x => x.ScoreSaberId);
		leaderboardEntryModelBuilder.Property(x => x.ScoreSaberId).HasMaxLength(20);
		leaderboardEntryModelBuilder.Property(x => x.CountryRank).IsRequired();
		leaderboardEntryModelBuilder.Property(x => x.Name).IsRequired();
		leaderboardEntryModelBuilder.Property(x => x.Pp).IsRequired();
	}
}