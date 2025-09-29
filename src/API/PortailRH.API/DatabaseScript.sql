IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Employees] (
    [Id] int NOT NULL IDENTITY,
    [LastName] nvarchar(max) NOT NULL,
    [FirstName] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [BirthDate] datetime2 NOT NULL,
    [Position] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250425132444_initialMigration', N'9.0.4');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250426125434_AddAdresseToEmploye', N'9.0.4');

EXEC sp_rename N'[Employees].[Position]', N'Poste', 'COLUMN';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250428072616_ChangePosteColumnTypeToString', N'9.0.4');

CREATE TABLE [Conges] (
    [Id] int NOT NULL IDENTITY,
    [TypeConge] nvarchar(max) NOT NULL,
    [DateDebut] datetime2 NOT NULL,
    [DateFin] datetime2 NOT NULL,
    [Statut] nvarchar(max) NOT NULL,
    [Motif] nvarchar(max) NOT NULL,
    [IdEmploye] int NOT NULL,
    CONSTRAINT [PK_Conges] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250428145939_conge', N'9.0.4');

CREATE TABLE [Candidatures] (
    [Id] int NOT NULL IDENTITY,
    [Nom] nvarchar(max) NOT NULL,
    [Prenom] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [PosteSouhaite] nvarchar(max) NOT NULL,
    [CVUrl] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Candidatures] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250429101609_Candidaturetable', N'9.0.4');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250429152836_RemoveCVUrlFromCandidature', N'9.0.4');

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Candidatures]') AND [c].[name] = N'CVUrl');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Candidatures] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Candidatures] DROP COLUMN [CVUrl];

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250429154306_deleteCVUrlFromcandidature', N'9.0.4');

CREATE TABLE [Projects] (
    [Id] int NOT NULL IDENTITY,
    [Type] nvarchar(max) NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [Priority] nvarchar(max) NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Projects] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250430102213_project', N'9.0.4');

ALTER TABLE [Candidatures] ADD [JobId] int NULL;

CREATE TABLE [Jobs] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [RequiredSkills] nvarchar(max) NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    [ContractType] nvarchar(max) NOT NULL,
    [Salary] decimal(18,2) NOT NULL,
    [ApplicationDeadline] datetime2 NOT NULL,
    [PublicationDate] datetime2 NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Jobs] PRIMARY KEY ([Id])
);

CREATE INDEX [IX_Candidatures_JobId] ON [Candidatures] ([JobId]);

ALTER TABLE [Candidatures] ADD CONSTRAINT [FK_Candidatures_Jobs_JobId] FOREIGN KEY ([JobId]) REFERENCES [Jobs] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250502085633_AddJobEntity', N'9.0.4');

CREATE TABLE [PaymentPolicies] (
    [Id] int NOT NULL IDENTITY,
    [TaxRate] decimal(18,2) NOT NULL,
    [SocialSecurityRate] decimal(18,2) NOT NULL,
    [OtherDeductions] decimal(18,2) NOT NULL,
    [PaymentDay] int NOT NULL,
    [ExcessDayPayment] int NOT NULL,
    [AllowedDays] int NOT NULL,
    [SickLeave] int NOT NULL,
    [PaidLeave] int NOT NULL,
    [UnpaidLeave] int NOT NULL,
    [Bereavement] int NOT NULL,
    [PersonalReasons] int NOT NULL,
    [Maternity] int NOT NULL,
    [Paternity] int NOT NULL,
    [RTT] int NOT NULL,
    [Other] int NOT NULL,
    CONSTRAINT [PK_PaymentPolicies] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250506095005_paymentpolicy', N'9.0.4');

ALTER TABLE [Candidatures] ADD [CvContent] varbinary(max) NOT NULL DEFAULT 0x;

ALTER TABLE [Candidatures] ADD [CvFileName] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [Candidatures] ADD [CvMimeType] nvarchar(max) NOT NULL DEFAULT N'';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250506144305_Addcvcandidature', N'9.0.4');

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Candidatures]') AND [c].[name] = N'CvContent');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Candidatures] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Candidatures] DROP COLUMN [CvContent];

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Candidatures]') AND [c].[name] = N'CvFileName');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Candidatures] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Candidatures] DROP COLUMN [CvFileName];

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Candidatures]') AND [c].[name] = N'CvMimeType');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Candidatures] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Candidatures] DROP COLUMN [CvMimeType];

ALTER TABLE [Employees] ADD [Salary] decimal(18,2) NOT NULL DEFAULT 0.0;

CREATE TABLE [Payslips] (
    [Id] int NOT NULL IDENTITY,
    [EmployeeId] int NOT NULL,
    [PaymentPolicyId] int NOT NULL,
    [BasicSalary] decimal(18,2) NOT NULL,
    [TaxDeduction] decimal(18,2) NOT NULL,
    [SocialSecurityDeduction] decimal(18,2) NOT NULL,
    [OtherDeductions] decimal(18,2) NOT NULL,
    [NetSalary] decimal(18,2) NOT NULL,
    [GeneratedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Payslips] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Payslips_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Payslips_PaymentPolicies_PaymentPolicyId] FOREIGN KEY ([PaymentPolicyId]) REFERENCES [PaymentPolicies] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Payslips_EmployeeId] ON [Payslips] ([EmployeeId]);

CREATE INDEX [IX_Payslips_PaymentPolicyId] ON [Payslips] ([PaymentPolicyId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250507110708_Addupdatedpayslip', N'9.0.4');

CREATE TABLE [Admins] (
    [Id] int NOT NULL IDENTITY,
    [Email] nvarchar(max) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Admins] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250516072556_AddAdminEntity', N'9.0.4');

ALTER TABLE [Candidatures] ADD [CVUrl] nvarchar(max) NOT NULL DEFAULT N'';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250703092715_AjoutCVUrlDansCandidature', N'9.0.4');

ALTER TABLE [Candidatures] DROP CONSTRAINT [FK_Candidatures_Jobs_JobId];

DROP INDEX [IX_Candidatures_JobId] ON [Candidatures];
DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Candidatures]') AND [c].[name] = N'JobId');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Candidatures] DROP CONSTRAINT [' + @var4 + '];');
UPDATE [Candidatures] SET [JobId] = 0 WHERE [JobId] IS NULL;
ALTER TABLE [Candidatures] ALTER COLUMN [JobId] int NOT NULL;
ALTER TABLE [Candidatures] ADD DEFAULT 0 FOR [JobId];
CREATE INDEX [IX_Candidatures_JobId] ON [Candidatures] ([JobId]);

ALTER TABLE [Candidatures] ADD CONSTRAINT [FK_Candidatures_Jobs_JobId] FOREIGN KEY ([JobId]) REFERENCES [Jobs] ([Id]) ON DELETE CASCADE;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250703094315_update', N'9.0.4');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250703095024_IdJob', N'9.0.4');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250703095744_JobId', N'9.0.4');

ALTER TABLE [Candidatures] DROP CONSTRAINT [FK_Candidatures_Jobs_JobId];

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Candidatures]') AND [c].[name] = N'JobId');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Candidatures] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Candidatures] ALTER COLUMN [JobId] int NULL;

ALTER TABLE [Candidatures] ADD CONSTRAINT [FK_Candidatures_Jobs_JobId] FOREIGN KEY ([JobId]) REFERENCES [Jobs] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250703102232_updates', N'9.0.4');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250703102454_RemoveJobIdFromCandidature', N'9.0.4');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250703103322_RemovesJobIdFromCandidature', N'9.0.4');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250703103918_DeleteJobIdFromCandidature', N'9.0.4');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250703104228_RemoveJobIdFromCandidatures', N'9.0.4');

EXEC sp_rename N'[Employees].[Salary]', N'Salaire', 'COLUMN';

EXEC sp_rename N'[Employees].[Poste]', N'Prenom', 'COLUMN';

EXEC sp_rename N'[Employees].[LastName]', N'NumeroTelephone', 'COLUMN';

EXEC sp_rename N'[Employees].[FirstName]', N'NumeroIdentification', 'COLUMN';

EXEC sp_rename N'[Employees].[BirthDate]', N'DateNaissance', 'COLUMN';

ALTER TABLE [Employees] ADD [DateEntree] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [Employees] ADD [EmailSecondaire] nvarchar(max) NULL;

ALTER TABLE [Employees] ADD [Etablissement] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [Employees] ADD [Fonction] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [Employees] ADD [MotDePasse] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [Employees] ADD [Nom] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [Employees] ADD [NumeroTelephoneSecondaire] nvarchar(max) NULL;

ALTER TABLE [Employees] ADD [SoldeConge] int NOT NULL DEFAULT 0;

ALTER TABLE [Employees] ADD [SoldeCongeMaladie] int NOT NULL DEFAULT 0;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250709071832_Updateemployee', N'9.0.4');

ALTER TABLE [Payslips] DROP CONSTRAINT [FK_Payslips_Employees_EmployeeId];

ALTER TABLE [Employees] DROP CONSTRAINT [PK_Employees];

EXEC sp_rename N'[Employees]', N'Employee', 'OBJECT';

ALTER TABLE [Employee] ADD CONSTRAINT [PK_Employee] PRIMARY KEY ([Id]);

ALTER TABLE [Payslips] ADD CONSTRAINT [FK_Payslips_Employee_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employee] ([Id]) ON DELETE CASCADE;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250709073432_columnsemployee', N'9.0.4');

EXEC sp_rename N'[Conges].[IdEmploye]', N'EmployeeId', 'COLUMN';

CREATE INDEX [IX_Conges_EmployeeId] ON [Conges] ([EmployeeId]);

ALTER TABLE [Conges] ADD CONSTRAINT [FK_Conges_Employee_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employee] ([Id]) ON DELETE CASCADE;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250711140423_AddCongeEmployeeRelation', N'9.0.4');

CREATE TABLE [SuiviConges] (
    [Id] int NOT NULL IDENTITY,
    [SoldeInitial] decimal(18,2) NOT NULL,
    [SoldeRestant] decimal(18,2) NOT NULL,
    [Annee] int NOT NULL,
    [Actif] bit NOT NULL,
    [EmployeeId] int NOT NULL,
    CONSTRAINT [PK_SuiviConges] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SuiviConges_Employee_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employee] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_SuiviConges_EmployeeId] ON [SuiviConges] ([EmployeeId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250714154052_AddSuiviConge', N'9.0.4');

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Employee]') AND [c].[name] = N'Etablissement');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Employee] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Employee] DROP COLUMN [Etablissement];

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Employee]') AND [c].[name] = N'Salaire');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Employee] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Employee] DROP COLUMN [Salaire];

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Employee]') AND [c].[name] = N'SoldeCongeMaladie');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Employee] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [Employee] DROP COLUMN [SoldeCongeMaladie];

EXEC sp_rename N'[Employee].[Fonction]', N'CreatedBy', 'COLUMN';

EXEC sp_rename N'[Employee].[DateEntree]', N'CreatedDate', 'COLUMN';

ALTER TABLE [SuiviConges] ADD [CreatedBy] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [SuiviConges] ADD [CreatedDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [Projects] ADD [CreatedBy] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [Projects] ADD [CreatedDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [Payslips] ADD [CreatedBy] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [Payslips] ADD [CreatedDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [PaymentPolicies] ADD [CreatedBy] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [PaymentPolicies] ADD [CreatedDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [Jobs] ADD [CreatedBy] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [Jobs] ADD [CreatedDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [Conges] ADD [CreatedBy] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [Conges] ADD [CreatedDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [Candidatures] ADD [CreatedBy] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [Candidatures] ADD [CreatedDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [Admins] ADD [CreatedBy] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [Admins] ADD [CreatedDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

CREATE TABLE [contrats] (
    [Id] int NOT NULL IDENTITY,
    [Fonction] nvarchar(max) NOT NULL,
    [Etablissement] nvarchar(max) NOT NULL,
    [DateDebut] datetime2 NOT NULL,
    [DateFin] datetime2 NULL,
    [Salaire] decimal(18,2) NOT NULL,
    [EmployeeId] int NOT NULL,
    [CreatedBy] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_contrats] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_contrats_Employee_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employee] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_contrats_EmployeeId] ON [contrats] ([EmployeeId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250727150029_Addcontrat', N'9.0.4');

ALTER TABLE [contrats] DROP CONSTRAINT [FK_contrats_Employee_EmployeeId];

ALTER TABLE [contrats] DROP CONSTRAINT [PK_contrats];

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[SuiviConges]') AND [c].[name] = N'CreatedBy');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [SuiviConges] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [SuiviConges] DROP COLUMN [CreatedBy];

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Projects]') AND [c].[name] = N'CreatedBy');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [Projects] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [Projects] DROP COLUMN [CreatedBy];

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Payslips]') AND [c].[name] = N'CreatedBy');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [Payslips] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [Payslips] DROP COLUMN [CreatedBy];

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PaymentPolicies]') AND [c].[name] = N'CreatedBy');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [PaymentPolicies] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [PaymentPolicies] DROP COLUMN [CreatedBy];

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Jobs]') AND [c].[name] = N'CreatedBy');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Jobs] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [Jobs] DROP COLUMN [CreatedBy];

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Conges]') AND [c].[name] = N'CreatedBy');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Conges] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [Conges] DROP COLUMN [CreatedBy];

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Candidatures]') AND [c].[name] = N'CreatedBy');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [Candidatures] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [Candidatures] DROP COLUMN [CreatedBy];

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Admins]') AND [c].[name] = N'CreatedBy');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [Admins] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [Admins] DROP COLUMN [CreatedBy];

EXEC sp_rename N'[contrats]', N'Contrats', 'OBJECT';

EXEC sp_rename N'[Employee].[CreatedBy]', N'Fonction', 'COLUMN';

EXEC sp_rename N'[Contrats].[CreatedBy]', N'Typecontrat', 'COLUMN';

EXEC sp_rename N'[Contrats].[IX_contrats_EmployeeId]', N'IX_Contrats_EmployeeId', 'INDEX';

ALTER TABLE [Employee] ADD [DateEntree] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [Employee] ADD [Etablissement] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [Employee] ADD [Salaire] decimal(18,2) NOT NULL DEFAULT 0.0;

ALTER TABLE [Employee] ADD [SoldeCongeMaladie] int NOT NULL DEFAULT 0;

ALTER TABLE [Contrats] ADD CONSTRAINT [PK_Contrats] PRIMARY KEY ([Id]);

ALTER TABLE [Contrats] ADD CONSTRAINT [FK_Contrats_Employee_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employee] ([Id]) ON DELETE CASCADE;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250727184027_Addemploye', N'9.0.4');

DECLARE @var17 sysname;
SELECT @var17 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Employee]') AND [c].[name] = N'SoldeCongeMaladie');
IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [Employee] DROP CONSTRAINT [' + @var17 + '];');
ALTER TABLE [Employee] DROP COLUMN [SoldeCongeMaladie];

ALTER TABLE [Employee] ADD [TypeContrat] nvarchar(max) NOT NULL DEFAULT N'';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250727185925_updateemployees', N'9.0.4');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250807160509_Addemployee', N'9.0.4');

COMMIT;
GO

