using GIDataAccess.Models;
using LoadDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlanGIDataAccess.Models;
using System.Collections.Generic;
using System.IO;

namespace DataAccess
{
    public class LoadDbContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public virtual DbSet<im_PlanGoodsIssue> im_PlanGoodsIssue { get; set; }
        public virtual DbSet<im_PlanGoodsIssueItem> im_PlanGoodsIssueItem { get; set; }
        public virtual DbSet<View_PlanGI> View_PlanGI { get; set; }

        public virtual DbSet<GetValueByColumn> GetValueByColumn { get; set; }
        public virtual DbSet<View_TruckLoadProcessStatus> View_TruckLoadProcessStatus { get; set; }

        #region TruckLoad
        public virtual DbSet<im_TruckLoad> im_TruckLoad { get; set; }
        public virtual DbSet<im_TruckLoadItem> im_TruckLoadItem { get; set; }
        public virtual DbSet<im_TruckLoadImages> im_TruckLoadImages { get; set; }
        #endregion

        public virtual DbSet<View_GoodsIssue> View_GoodsIssue { get; set; }
        public virtual DbSet<View_GoodsIssueTruckload> View_GoodsIssueTruckload { get; set; }

        public virtual DbSet<im_Pack> im_Pack { get; set; }
        public virtual DbSet<im_PackItem> im_PackItem { get; set; }
        public DbSet<im_PlanGoodsIssueItemSN> im_PlanGoodsIssueItemSN { get; set; }
        public DbSet<View_TLCheckPlanGI> View_TLCheckPlanGI { get; set; }
        public DbSet<View_LoadPlan> View_LoadPlan { get; set; }
        public DbSet<im_Task> IM_Task{ get; set; }
        public DbSet<im_TaskItem> IM_TaskItem { get; set; }
        public DbSet<im_GoodsIssue> IM_GoodsIssue { get; set; }
        public DbSet<im_GoodsIssueItemLocation> IM_GoodsIssueItemLocation { get; set; }
        public virtual DbSet<View_PrintOutTruckLoad> View_PrintOutTruckLoad { get; set; }
        public virtual DbSet<View_PrintOutPlanGIV2> View_PrintOutPlanGIV2 { get; set; }
        public virtual DbSet<View_LoadPlan_V2> View_LoadPlan_V2 { get; set; }
        public virtual DbSet<View_Load_returntote> View_Load_returntote { get; set; }
        public virtual DbSet<im_TruckLoad_ReturnTote> im_TruckLoad_ReturnTote { get; set; }
        public virtual DbSet<im_TruckLoad_ReturnTote_Tran> im_TruckLoad_ReturnTote_Tran { get; set; }
        public virtual DbSet<View_Load_Cartontote> View_Load_Cartontote { get; set; }
        public virtual DbSet<im_RollCageOrder> im_RollCageOrder { get; set; }
        public virtual DbSet<View_RPT_Handover> View_RPT_Handover { get; set; }
        public virtual DbSet<View_RPT_Truck_Menifest> View_RPT_Truck_Menifest { get; set; }
        public virtual DbSet<View_RPT_Delivery_Note> View_RPT_Delivery_Note { get; set; }
        public virtual DbSet<View_ToteReturnTMS> View_ToteReturnTMS { get; set; }
        public virtual DbSet<View_RPT_ToteReturnTMS> View_RPT_ToteReturnTMS { get; set; }
        public virtual DbSet<View_CheckReport_Status> View_CheckReport_Status { get; set; }
        public virtual DbSet<im_PlanGoodsIssue_Ref> im_PlanGoodsIssue_Ref { get; set; }
        public virtual DbSet<View_ShortShip> View_ShortShip { get; set; }
        public virtual DbSet<View_Taskitem_with_Truckload> View_Taskitem_with_Truckload { get; set; }
        public virtual DbSet<View_ShortShipV2> View_ShortShipV2 { get; set; }
        public virtual DbSet<View_ReportShortShip> View_ReportShortShip { get; set; }
        public virtual DbSet<wm_TagOutShortShip> wm_TagOutShortShip { get; set; }
        public virtual DbSet<wm_TagOutItemShortShip> wm_TagOutItemShortShip { get; set; }
        public virtual DbSet<View_TagOutItemShortship> View_TagOutItemShortship { get; set; }
        public virtual DbSet<View_Trace_picking> View_Trace_picking { get; set; }
        public virtual DbSet<View_Trace_Loading> View_Trace_Loading { get; set; }

        public virtual DbSet<log_api_reponse> log_api_reponse { get; set; }
        public virtual DbSet<log_api_request> log_api_request { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false);

                var configuration = builder.Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public int Rating { get; set; }
        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
