using PropTrade.Api.Entities;
using System.Data.Entity;

namespace PropTrade.Api.DataAccess
{
    public class PropTradeDbContext : DbContext
    {
        /// <summary>
        /// Static constructor - initialize the database
        /// </summary>
        static PropTradeDbContext()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<PropTradeDbContext>());            
        }

        /// <summary>
        /// Initializes a new instance of of the AdBrain db context
        /// </summary>
        public PropTradeDbContext() : base("PropTradeDbContext") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Offer> Offers { get; set; }

        public DbSet<Property> Properties { get; set; }
    }
}