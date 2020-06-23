-- -----------------------------------------------------------------------------------------
-- Abstract: Create and setup tables
-- -----------------------------------------------------------------------------------------

-- -----------------------------------------------------------------------------------------
-- Options
-- -----------------------------------------------------------------------------------------
USE dbBibleBooks;      -- Get out of the master database
SET NOCOUNT ON;		   -- Report only errors

-- Drop Table Statements for testing

IF OBJECT_ID ('TUsers')		IS NOT NULL		DROP TABLE TUsers


-- -----------------------------------------------------------------------------------------
--	Step #1 : Create tables
-- -----------------------------------------------------------------------------------------

CREATE TABLE TUsers
(
	 intUserID				INTEGER			NOT NULL
	,strUsername			VARCHAR(255)	NOT NULL
	,strPassword			VARCHAR(255)	NOT NULL
	,CONSTRAINT TUsers_PK PRIMARY KEY ( intUserID )
)