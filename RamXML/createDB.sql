-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/04/2014 01:29:40
-- Generated from EDMX file: C:\Users\helll_000\Documents\Visual Studio 2013\Projects\RamXML\RamXML\data.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [xmldb];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_docconcept]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[concept] DROP CONSTRAINT [FK_docconcept];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[concept]', 'U') IS NOT NULL
    DROP TABLE [dbo].[concept];
GO
IF OBJECT_ID(N'[dbo].[concepts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[concepts];
GO
IF OBJECT_ID(N'[dbo].[doc]', 'U') IS NOT NULL
    DROP TABLE [dbo].[doc];
GO
IF OBJECT_ID(N'[dbo].[dropdown]', 'U') IS NOT NULL
    DROP TABLE [dbo].[dropdown];
GO
IF OBJECT_ID(N'[dbo].[nodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[nodes];
GO
IF OBJECT_ID(N'[dbo].[ontology]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ontology];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'concept'
CREATE TABLE [dbo].[concept] (
    [id] int IDENTITY(1,1) NOT NULL,
    [id_Doc] int  NOT NULL,
    [Descriptor] varchar(max)  NULL,
    [Df] varchar(max)  NULL,
    [Ds] varchar(max)  NULL,
    [Es] varchar(max)  NULL
);
GO

-- Creating table 'doc'
CREATE TABLE [dbo].[doc] (
    [id] int IDENTITY(1,1) NOT NULL,
    [name] varchar(max)  NULL
);
GO

-- Creating table 'dropdown'
CREATE TABLE [dbo].[dropdown] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [value] varchar(max)  NOT NULL
);
GO

-- Creating table 'ontology'
CREATE TABLE [dbo].[ontology] (
    [id] int IDENTITY(1,1) NOT NULL,
    [id_concept] int  NOT NULL,
    [dropdown1] varchar(max)  NOT NULL,
    [dropdown2] varchar(max)  NOT NULL,
    [dropdown3] varchar(max)  NOT NULL,
    [id_doc] int  NOT NULL
);
GO

-- Creating table 'concepts'
CREATE TABLE [dbo].[concepts] (
    [id] int IDENTITY(1,1) NOT NULL,
    [id_doc] int  NOT NULL,
    [parent] int  NULL
);
GO

-- Creating table 'nodes'
CREATE TABLE [dbo].[nodes] (
    [id] int IDENTITY(1,1) NOT NULL,
    [id_concept] int  NOT NULL,
    [nodeName] varchar(max)  NOT NULL,
    [value] varchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'concept'
ALTER TABLE [dbo].[concept]
ADD CONSTRAINT [PK_concept]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'doc'
ALTER TABLE [dbo].[doc]
ADD CONSTRAINT [PK_doc]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'dropdown'
ALTER TABLE [dbo].[dropdown]
ADD CONSTRAINT [PK_dropdown]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id] in table 'ontology'
ALTER TABLE [dbo].[ontology]
ADD CONSTRAINT [PK_ontology]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'concepts'
ALTER TABLE [dbo].[concepts]
ADD CONSTRAINT [PK_concepts]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'nodes'
ALTER TABLE [dbo].[nodes]
ADD CONSTRAINT [PK_nodes]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [id_Doc] in table 'concept'
ALTER TABLE [dbo].[concept]
ADD CONSTRAINT [FK_docconcept]
    FOREIGN KEY ([id_Doc])
    REFERENCES [dbo].[doc]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_docconcept'
CREATE INDEX [IX_FK_docconcept]
ON [dbo].[concept]
    ([id_Doc]);
GO

-- Creating foreign key on [id_doc] in table 'concepts'
ALTER TABLE [dbo].[concepts]
ADD CONSTRAINT [FK_docconcepts]
    FOREIGN KEY ([id_doc])
    REFERENCES [dbo].[doc]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_docconcepts'
CREATE INDEX [IX_FK_docconcepts]
ON [dbo].[concepts]
    ([id_doc]);
GO

-- Creating foreign key on [id_concept] in table 'nodes'
ALTER TABLE [dbo].[nodes]
ADD CONSTRAINT [FK_conceptsnodes]
    FOREIGN KEY ([id_concept])
    REFERENCES [dbo].[concepts]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_conceptsnodes'
CREATE INDEX [IX_FK_conceptsnodes]
ON [dbo].[nodes]
    ([id_concept]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------