using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BankCodeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    acqId = table.Column<int>(type: "int", nullable: false),
                    CassoAccountID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Main_office = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Representative = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vision = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoreValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainActivity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Interests = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VolunteerExperience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VolunteerObjectives = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SDGs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SDGName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SDGs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<bool>(type: "bit", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkPlace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Education = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Skill = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hobby = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Strength = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VolunteerExperience = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VolunteerGoal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreateQrCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateQrCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreateQrCodes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrganizationMember",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationMember_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationMember_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Projects_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocialNetworks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SocialNetworksLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialNetworks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialNetworks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSDGs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SDGId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSDGs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSDGs_SDGs_SDGId",
                        column: x => x.SDGId,
                        principalTable: "SDGs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSDGs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FundTarget = table.Column<long>(type: "bigint", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdateLog = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campaigns_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Campaigns_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Campaigns_Users_CreateBy",
                        column: x => x.CreateBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CreateProjectRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateProjectRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreateProjectRequests_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreateProjectRequests_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreateProjectRequests_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountingBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CampaignId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountingBooks_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Debt = table.Column<decimal>(type: "money", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignMembers_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CampaignMembers_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreateCampaignRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateCampaignRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreateCampaignRequests_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreateCampaignRequests_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CreateCampaignRequests_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MoveNextCampaignStatusRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CampaignID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusOfCampaign = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoveNextCampaignStatusRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoveNextCampaignStatusRequests_Campaigns_CampaignID",
                        column: x => x.CampaignID,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MoveNextCampaignStatusRequests_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MoveNextCampaignStatusRequests_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RequestForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpectedMoney = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestForms_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RequestForms_RequestTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "RequestTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RequestForms_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransactionLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateQrCodeId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CassoTransactionId = table.Column<long>(type: "bigint", nullable: true),
                    tid = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionLogs_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TransactionLogs_CreateQrCodes_CreateQrCodeId",
                        column: x => x.CreateQrCodeId,
                        principalTable: "CreateQrCodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CreateProjectFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateProjectFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreateProjectFiles_CreateProjectRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "CreateProjectRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreateProjectRequestApproveHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateProjectRequestId = table.Column<int>(type: "int", nullable: false),
                    ApproverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateOfApproval = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAllowed = table.Column<bool>(type: "bit", nullable: false),
                    FeedBack = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateProjectRequestApproveHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreateProjectRequestApproveHistories_CreateProjectRequests_CreateProjectRequestId",
                        column: x => x.CreateProjectRequestId,
                        principalTable: "CreateProjectRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreateProjectRequestApproveHistories_Users_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignRequestApproveHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignRequestId = table.Column<int>(type: "int", nullable: false),
                    ApproverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateOfApproval = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAllowed = table.Column<bool>(type: "bit", nullable: false),
                    FeedBack = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignRequestApproveHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignRequestApproveHistories_CreateCampaignRequests_CampaignRequestId",
                        column: x => x.CampaignRequestId,
                        principalTable: "CreateCampaignRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CampaignRequestApproveHistories_Users_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreateCampaignFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateCampaignFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreateCampaignFiles_CreateCampaignRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "CreateCampaignRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MoveNextCampaignStatusRequestHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoveNextCampaignStatusRequestId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateOfApproval = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAllowed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoveNextCampaignStatusRequestHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoveNextCampaignStatusRequestHistories_MoveNextCampaignStatusRequests_MoveNextCampaignStatusRequestId",
                        column: x => x.MoveNextCampaignStatusRequestId,
                        principalTable: "MoveNextCampaignStatusRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoveNextCampaignStatusRequestHistories_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApproveProcesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApproveNumber = table.Column<int>(type: "int", nullable: false),
                    ApproveStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    ApproverId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApproveProcesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApproveProcesses_RequestForms_RequestId",
                        column: x => x.RequestId,
                        principalTable: "RequestForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApproveProcesses_Users_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachmentFiles_RequestForms_RequestId",
                        column: x => x.RequestId,
                        principalTable: "RequestForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApproveId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vouchers_ApproveProcesses_ApproveId",
                        column: x => x.ApproveId,
                        principalTable: "ApproveProcesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Banks",
                columns: new[] { "Id", "AccountNumber", "BankCodeName", "CassoAccountID", "OwnerName", "acqId" },
                values: new object[,]
                {
                    { 1, "1016161976", "VietComBank", 123L, "Nguyen Van Duc", 970436 },
                    { 2, "4270774590", "BIDV", 222L, "Bui Minh Manh", 970418 }
                });

            migrationBuilder.InsertData(
                table: "RequestTypes",
                columns: new[] { "Id", "TypeName" },
                values: new object[,]
                {
                    { 1, "Pre-Pay" },
                    { 2, "Payment" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "15db7f37-5dbc-4035-9b00-a0af4c3fe8bb", "ba58588f-f626-41a0-8fca-b74481367335", "Admin", "ADMIN" },
                    { "15db7f37-5dbc-4035-9b00-a0af4c3fe8bd", "ba58588f-f626-41a0-8fca-b74481367337", "Donor", "DONOR" },
                    { "205d4496-4ac8-40d9-84b9-e09e1ada7a49", "acccef8b-20f3-4de0-8ee9-5a3690f094ed", "Project Manager", "PROJECT MANAGER" },
                    { "4e7b2c09-e0b0-4ddd-9694-ebf3e21e2472", "1a777fbf-24db-4247-bd76-db376d703ea9", "Volunteer Leader", "VOLUNTEER LEADER" },
                    { "83292e2c-6c86-4153-bdc5-760d05ec2293", "606fea67-ae89-4b3f-ac93-ccceda6fc85f", "Accounting", "ACCOUNTING" },
                    { "83292e2c-6c86-4153-bdc5-760d05ec2295", "606fea67-ae89-4b3f-ac93-ccceda6fc85g", "Volunteer", "VOLUNTEER" },
                    { "83292e2c-6c86-4153-bdc5-760d05ec2299", "606fea67-ae89-4b3f-ac93-ccceda6fc85h", "Project Management Board", "PROJECT MANAGEMENT BOARD" }
                });

            migrationBuilder.InsertData(
                table: "SDGs",
                columns: new[] { "Id", "SDGName" },
                values: new object[,]
                {
                    { 1, "No Poverty" },
                    { 2, "Zero Hunger" },
                    { 3, "Good Health And Well-Being" },
                    { 4, "Quality Education" },
                    { 5, "Gender Equality" },
                    { 6, "Clean Water And Sanitation" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "Address", "Bio", "BirthDate", "ConcurrencyStamp", "Education", "Email", "EmailConfirmed", "FirstName", "Gender", "Hobby", "Image", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Skill", "Strength", "TwoFactorEnabled", "UserName", "VolunteerExperience", "VolunteerGoal", "WorkPlace" },
                values: new object[,]
                {
                    { "01234567-89ab-cdef-1234-567890123", 0, "777 Đường Điện Biên Phủ, Quận 3, TP.HCM", "Luật sư, bảo vệ công lý và lẽ phải.", new DateOnly(1993, 2, 14), "f44bbada-65d8-4f66-84bd-9f5a0d356dcb", "Đại học Luật TP.HCM, Luật", "user9@falofinancial.com", true, "Trương", false, "Du lịch, Âm nhạc", null, "Thị Thu", false, null, "USER9@FALOFINANCIAL.COM", "USER9@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAELg0I6FNFMtpNqN8vj2DlxJ3cEezxFsvb7OsKpJWDErIVr8CJzGH1rtTxOVe9XtxBg==", null, false, "8a344601-218a-45a1-acc1-69e194bc3ca8", "Tư vấn pháp lý, Soạn thảo văn bản pháp luật", "Công bằng, Chính trực", false, "user9@falofinancial.com", "Tư vấn pháp lý miễn phí cho người nghèo", "Đóng góp cho sự phát triển của pháp luật.", "Công ty Luật" },
                    { "78901234-5678-90ab-cdef-123456789", 0, "444 Đường Cách Mạng Tháng 8, Quận 10, TP.HCM", "Giáo viên yêu nghề, mong muốn truyền đạt kiến thức cho học sinh.", new DateOnly(1987, 4, 8), "64fcfbf2-5d86-4230-9533-81fb2b48b5f0", "Đại học Sư phạm TP.HCM, Ngữ văn", "user6@falofinancial.com", true, "Võ", true, "Đọc sách, Du lịch", null, "Văn Nam", false, null, "USER6@FALOFINANCIAL.COM", "USER6@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAELg0I6FNFMtpNqN8vj2DlxJ3cEezxFsvb7OsKpJWDErIVr8CJzGH1rtTxOVe9XtxBg==", null, false, "69e19fed-7af6-4639-941e-8a96ced026a4", "Giảng dạy, Truyền đạt kiến thức", "Nhiệt tình, Trách nhiệm", false, "user6@falofinancial.com", "Tham gia dạy học tình thương", "Cống hiến cho sự nghiệp giáo dục.", "Trường THPT Lê Hồng Phong" },
                    { "89012345-6789-abcd-ef12-345678901", 0, "555 Đường Nguyễn Thị Minh Khai, Quận 3, TP.HCM", "Nhà thiết kế thời trang, yêu cái đẹp và sự sáng tạo.", new DateOnly(1994, 11, 12), "85e33542-d155-4f66-9d9a-83b5eb3bc0ba", "Đại học Mỹ thuật TP.HCM, Thiết kế Thời trang", "user7@falofinancial.com", true, "Đỗ", false, "Xem phim, Nghe nhạc", null, "Thị Ngọc", false, null, "USER7@FALOFINANCIAL.COM", "USER7@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAELg0I6FNFMtpNqN8vj2DlxJ3cEezxFsvb7OsKpJWDErIVr8CJzGH1rtTxOVe9XtxBg==", null, false, "61af8e6d-863f-4906-90de-e796c3575c75", "Thiết kế, May vá", "Sáng tạo, Thẩm mỹ", false, "user7@falofinancial.com", "Tham gia thiết kế trang phục cho chương trình từ thiện", "Góp phần làm đẹp cho đời.", "Công ty Thiết kế Thời trang" },
                    { "90123456-789a-bcde-f123-456789012", 0, "666 Đường Hai Bà Trưng, Quận 1, TP.HCM", "Chuyên viên phân tích chứng khoán, đam mê thị trường tài chính.", new DateOnly(1991, 8, 22), "c5a3df2a-1ea9-4936-9861-d2c6afc6e427", "Đại học Ngân hàng TP.HCM, Chứng khoán", "user8@falofinancial.com", true, "Lý", true, "Chơi thể thao, Đọc sách", null, "Văn Phong", false, null, "USER8@FALOFINANCIAL.COM", "USER8@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAELg0I6FNFMtpNqN8vj2DlxJ3cEezxFsvb7OsKpJWDErIVr8CJzGH1rtTxOVe9XtxBg==", null, false, "a8dd8874-4c10-4eee-909d-b359f526e45d", "Phân tích chứng khoán, Đầu tư", "Tư duy logic, Phân tích", false, "user8@falofinancial.com", "Tham gia tư vấn tài chính cho người dân", "Giúp mọi người hiểu rõ hơn về tài chính.", "Công ty Chứng khoán" },
                    { "a1b2c3d4-e5f6-7890-1234-567890abcdef", 0, "123 Đường Chính, Thành phố A", "Lập trình viên phần mềm đam mê công nghệ web.", new DateOnly(1990, 5, 15), "021008a7-a0d7-42cc-8c6a-957e14541e4b", "Đại học Bách Khoa, Khoa Công nghệ Thông tin", "admin@falofinancial.com", true, "Nguyễn", true, "Đi bộ đường dài, Nhiếp ảnh", null, "Văn An", false, null, "ADMIN@FALOFINANCIAL.COM", "ADMIN@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAELg0I6FNFMtpNqN8vj2DlxJ3cEezxFsvb7OsKpJWDErIVr8CJzGH1rtTxOVe9XtxBg==", null, false, "d1f58327-81b7-4629-9663-9ecdeb32f5e3", "C#, .NET, JavaScript", "Giải quyết vấn đề, Làm việc nhóm", false, "admin@falofinancial.com", "Tình nguyện viên tại Mái ấm Tình Thương", "Đóng góp cho sự phát triển cộng đồng địa phương.", "Công ty ABC" },
                    { "b2c3d4e5-f678-9012-3456-7890abcdef1", 0, "456 Đường Số 2, Thành phố B", "Nhân viên văn phòng năng động.", new DateOnly(1988, 7, 20), "96292080-7d1a-4054-a91d-74981141d931", "Cao đẳng Kinh tế, Quản trị văn phòng", "user1@falofinancial.com", true, "Lê", false, "Xem phim, Nghe nhạc", null, "Thị Bình", false, null, "USER1@FALOFINANCIAL.COM", "USER1@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAELg0I6FNFMtpNqN8vj2DlxJ3cEezxFsvb7OsKpJWDErIVr8CJzGH1rtTxOVe9XtxBg==", null, false, "3bab4250-cccb-46e8-8578-d1d4ef4d7635", "Soạn thảo văn bản, Quản lý hồ sơ, Giao tiếp tốt", "Cẩn thận, Chu đáo", false, "user1@falofinancial.com", null, "Tham gia các hoạt động thiện nguyện giúp đỡ cộng đồng.", "Công ty XYZ" },
                    { "c3d4e5f6-7890-1234-5678-90abcdef12", 0, "789 Đường 30/4, Thành phố CT", "Lập trình viên tự do, thích khám phá công nghệ mới.", new DateOnly(1995, 3, 10), "6edc9ac9-6054-4a7e-ab56-d9d4459dc9e4", "Đại học Cần Thơ, Công nghệ Phần mềm", "user2@falofinancial.com", true, "Cao", true, "Đọc sách, Chơi thể thao", null, "Văn Tuấn", false, null, "USER2@FALOFINANCIAL.COM", "USER2@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAELg0I6FNFMtpNqN8vj2DlxJ3cEezxFsvb7OsKpJWDErIVr8CJzGH1rtTxOVe9XtxBg==", null, false, "b7e4b843-c552-4ba9-8ffd-8f87bfcb179a", "PHP, MySQL, Laravel", "Tự học, Sáng tạo", false, "user2@falofinancial.com", "Tham gia dự án mã nguồn mở", "Đóng góp cho cộng đồng lập trình viên.", "Freelancer" },
                    { "d4e5f678-9012-3456-7890-abcdef123", 0, "1011 Đường Lê Lợi, Quận 1, TP.HCM", "Thích tham gia các hoạt động xã hội.", new DateOnly(1992, 9, 25), "d1f4743b-afa4-4d9e-9b73-04802042cb65", "Đại học Kinh tế TP.HCM, Tài chính Ngân hàng", "user3@falofinancial.com", true, "Trần", false, "Du lịch, Đọc sách", null, "Thị Diễm", false, null, "USER3@FALOFINANCIAL.COM", "USER3@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAELg0I6FNFMtpNqN8vj2DlxJ3cEezxFsvb7OsKpJWDErIVr8CJzGH1rtTxOVe9XtxBg==", null, false, "6e9cbe23-2cc1-4865-8650-2d8e188db2bc", "Phân tích tài chính, Tư vấn đầu tư", "Giao tiếp, Thuyết trình", false, "user3@falofinancial.com", "Tình nguyện viên dạy học cho trẻ em nghèo", "Góp phần xây dựng một xã hội tốt đẹp hơn.", "Ngân hàng ACB" },
                    { "e5f67890-1234-5678-90ab-cdef12345", 0, "222 Đường Nguyễn Huệ, Quận 3, TP.HCM", "Kỹ sư cầu nối, yêu thích công việc và cuộc sống.", new DateOnly(1985, 12, 5), "f4a71efc-893f-4ff4-90dc-88714957b5f6", "Đại học Giao thông Vận tải, Kỹ thuật Cầu đường", "user4@falofinancial.com", true, "Phạm", true, "Chơi game, Xem phim", null, "Văn Hoàng", false, null, "USER4@FALOFINANCIAL.COM", "USER4@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAELg0I6FNFMtpNqN8vj2DlxJ3cEezxFsvb7OsKpJWDErIVr8CJzGH1rtTxOVe9XtxBg==", null, false, "1a3501ba-f7c5-4f41-b112-6671d0a4daa4", "Thiết kế cầu đường, Quản lý dự án", "Chịu khó, Ham học hỏi", false, "user4@falofinancial.com", "Tham gia xây dựng cầu dân sinh", "Mang lại niềm vui cho mọi người.", "Công ty FPT" },
                    { "f6789012-3456-7890-abcd-ef1234567", 0, "333 Đường Pasteur, Quận 1, TP.HCM", "Y tá tận tâm với nghề.", new DateOnly(1998, 6, 18), "fa822749-837e-486f-9758-e2dc6223fd1f", "Đại học Y Dược TP.HCM, Điều dưỡng", "user5@falofinancial.com", true, "Hồ", false, "Nấu ăn, Làm bánh", null, "Thị Mai", false, null, "USER5@FALOFINANCIAL.COM", "USER5@FALOFINANCIAL.COM", "AQAAAAIAAYagAAAAELg0I6FNFMtpNqN8vj2DlxJ3cEezxFsvb7OsKpJWDErIVr8CJzGH1rtTxOVe9XtxBg==", null, false, "954ebdc0-d191-4929-8000-66b014916471", "Chăm sóc bệnh nhân, Sơ cứu", "Kiên nhẫn, Yêu thương con người", false, "user5@falofinancial.com", "Tình nguyện viên tại trạm y tế xã", "Giúp đỡ những người bệnh tật.", "Bệnh viện Chợ Rẫy" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "83292e2c-6c86-4153-bdc5-760d05ec2295", "01234567-89ab-cdef-1234-567890123" },
                    { "83292e2c-6c86-4153-bdc5-760d05ec2293", "78901234-5678-90ab-cdef-123456789" },
                    { "4e7b2c09-e0b0-4ddd-9694-ebf3e21e2472", "89012345-6789-abcd-ef12-345678901" },
                    { "4e7b2c09-e0b0-4ddd-9694-ebf3e21e2472", "90123456-789a-bcde-f123-456789012" },
                    { "15db7f37-5dbc-4035-9b00-a0af4c3fe8bb", "a1b2c3d4-e5f6-7890-1234-567890abcdef" },
                    { "205d4496-4ac8-40d9-84b9-e09e1ada7a49", "b2c3d4e5-f678-9012-3456-7890abcdef1" },
                    { "205d4496-4ac8-40d9-84b9-e09e1ada7a49", "c3d4e5f6-7890-1234-5678-90abcdef12" },
                    { "83292e2c-6c86-4153-bdc5-760d05ec2299", "d4e5f678-9012-3456-7890-abcdef123" },
                    { "83292e2c-6c86-4153-bdc5-760d05ec2299", "e5f67890-1234-5678-90ab-cdef12345" },
                    { "83292e2c-6c86-4153-bdc5-760d05ec2293", "f6789012-3456-7890-abcd-ef1234567" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountingBooks_CampaignId",
                table: "AccountingBooks",
                column: "CampaignId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApproveProcesses_ApproverId",
                table: "ApproveProcesses",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_ApproveProcesses_RequestId",
                table: "ApproveProcesses",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentFiles_RequestId",
                table: "AttachmentFiles",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Banks_AccountNumber",
                table: "Banks",
                column: "AccountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Banks_CassoAccountID",
                table: "Banks",
                column: "CassoAccountID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CampaignMembers_CampaignId",
                table: "CampaignMembers",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignMembers_RoleId",
                table: "CampaignMembers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignMembers_UserId",
                table: "CampaignMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignRequestApproveHistories_ApproverId",
                table: "CampaignRequestApproveHistories",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignRequestApproveHistories_CampaignRequestId",
                table: "CampaignRequestApproveHistories",
                column: "CampaignRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_BankId",
                table: "Campaigns",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_CreateBy",
                table: "Campaigns",
                column: "CreateBy");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_ProjectId",
                table: "Campaigns",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateCampaignFiles_RequestId",
                table: "CreateCampaignFiles",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateCampaignRequests_CampaignId",
                table: "CreateCampaignRequests",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateCampaignRequests_ReceiverId",
                table: "CreateCampaignRequests",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateCampaignRequests_SenderId",
                table: "CreateCampaignRequests",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateProjectFiles_RequestId",
                table: "CreateProjectFiles",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateProjectRequestApproveHistories_ApproverId",
                table: "CreateProjectRequestApproveHistories",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateProjectRequestApproveHistories_CreateProjectRequestId",
                table: "CreateProjectRequestApproveHistories",
                column: "CreateProjectRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateProjectRequests_ProjectId",
                table: "CreateProjectRequests",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateProjectRequests_ReceiverId",
                table: "CreateProjectRequests",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateProjectRequests_SenderId",
                table: "CreateProjectRequests",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateQrCodes_UserId",
                table: "CreateQrCodes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveNextCampaignStatusRequestHistories_MoveNextCampaignStatusRequestId",
                table: "MoveNextCampaignStatusRequestHistories",
                column: "MoveNextCampaignStatusRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveNextCampaignStatusRequestHistories_ReceiverId",
                table: "MoveNextCampaignStatusRequestHistories",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveNextCampaignStatusRequests_CampaignID",
                table: "MoveNextCampaignStatusRequests",
                column: "CampaignID");

            migrationBuilder.CreateIndex(
                name: "IX_MoveNextCampaignStatusRequests_ReceiverId",
                table: "MoveNextCampaignStatusRequests",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveNextCampaignStatusRequests_SenderId",
                table: "MoveNextCampaignStatusRequests",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMember_OrganizationId",
                table: "OrganizationMember",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMember_UserId",
                table: "OrganizationMember",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CreatedBy",
                table: "Projects",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OrganizationId",
                table: "Projects",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestForms_CampaignId",
                table: "RequestForms",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestForms_CreatedBy",
                table: "RequestForms",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RequestForms_TypeId",
                table: "RequestForms",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SocialNetworks_UserId",
                table: "SocialNetworks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLogs_CampaignId",
                table: "TransactionLogs",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLogs_CassoTransactionId",
                table: "TransactionLogs",
                column: "CassoTransactionId",
                unique: true,
                filter: "[CassoTransactionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLogs_CreateQrCodeId",
                table: "TransactionLogs",
                column: "CreateQrCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserSDGs_SDGId",
                table: "UserSDGs",
                column: "SDGId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSDGs_UserId",
                table: "UserSDGs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_ApproveId",
                table: "Vouchers",
                column: "ApproveId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountingBooks");

            migrationBuilder.DropTable(
                name: "AttachmentFiles");

            migrationBuilder.DropTable(
                name: "CampaignMembers");

            migrationBuilder.DropTable(
                name: "CampaignRequestApproveHistories");

            migrationBuilder.DropTable(
                name: "CreateCampaignFiles");

            migrationBuilder.DropTable(
                name: "CreateProjectFiles");

            migrationBuilder.DropTable(
                name: "CreateProjectRequestApproveHistories");

            migrationBuilder.DropTable(
                name: "MoveNextCampaignStatusRequestHistories");

            migrationBuilder.DropTable(
                name: "OrganizationMember");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SocialNetworks");

            migrationBuilder.DropTable(
                name: "TransactionLogs");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserSDGs");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "CreateCampaignRequests");

            migrationBuilder.DropTable(
                name: "CreateProjectRequests");

            migrationBuilder.DropTable(
                name: "MoveNextCampaignStatusRequests");

            migrationBuilder.DropTable(
                name: "CreateQrCodes");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "SDGs");

            migrationBuilder.DropTable(
                name: "ApproveProcesses");

            migrationBuilder.DropTable(
                name: "RequestForms");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "RequestTypes");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
