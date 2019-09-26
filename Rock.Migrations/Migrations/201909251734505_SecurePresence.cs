﻿// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
namespace Rock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    ///
    /// </summary>
    public partial class SecurePresence : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            RestSecurePresenceUser();
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
        }

        /// <summary>
        /// ED: Presence User Security Changes
        /// </summary>
        private void RestSecurePresenceUser()
        {
            Sql( @"
                IF NOT EXISTS (SELECT [Id] FROM [RestController] WHERE [ClassName] = 'Rock.Rest.Controllers.AttendancesController') 

                IF NOT EXISTS (SELECT [Id] FROM [RestController] WHERE [ClassName] = 'Rock.Rest.Controllers.GroupsController') 


                IF NOT EXISTS (SELECT [Id] FROM [RestAction] WHERE [ApiId] = 'GETapi/Groups/GroupTypeCheckinConfiguration/{groupTypeGuid}')

                IF NOT EXISTS (SELECT [Id] FROM [RestAction] WHERE [ApiId] = 'GETapi/PersonAlias') 

                IF NOT EXISTS (SELECT [Id] FROM [RestAction] WHERE [ApiId] = 'GETapi/PersonAlias/{id}') 

                IF NOT EXISTS (SELECT [Id] FROM [RestAction] WHERE [ApiId] = 'POSTapi/Attendances/Import') 

                IF NOT EXISTS (SELECT [Id] FROM [RestAction] WHERE [ApiId] = 'PUTapi/Attendances/AddAttendance?groupId={groupId}&locationId={locationId}&scheduleId={scheduleId}&occurrenceDate={occurrenceDate}&personId={personId}&personAliasId={personAliasId}') 

                DECLARE @RestActionEntityTypeId INT = (SELECT [Id] FROM [EntityType] WHERE [Guid] = 'D4F7F055-5351-4ADF-9F8D-4802CAD6CC9D')
                DECLARE @GetPersonAliasRestActionId INT = (SELECT [Id] FROM [RestAction] WHERE [ApiId] = 'GETapi/PersonAlias')
                DECLARE @GetGroupTypeCheckinConfigurationId INT = (SELECT [Id] FROM [RestAction] WHERE [ApiId] = 'GETapi/Groups/GroupTypeCheckinConfiguration/{groupTypeGuid}')
                DECLARE @PutAddAttendanceId INT = (SELECT [Id] FROM [RestAction] WHERE [ApiId] = 'PUTapi/Attendances/AddAttendance?groupId={groupId}&locationId={locationId}&scheduleId={scheduleId}&occurrenceDate={occurrenceDate}&personId={personId}&personAliasId={personAliasId}')
                DECLARE @PostAttendancesImportId INT = (SELECT [Id] FROM [RestAction] WHERE [ApiId] = 'POSTapi/Attendances/Import')
                DECLARE @presencePersonAliasId INT = (SELECT [Id] FROM [PersonAlias] WHERE [AliasPersonGuid] = '86CF11D9-66BC-4CE0-9037-F8AFCBCD608A')

                -- If there is already user defined security on GETapi/PersonAlias/{id} don't change it.
                IF NOT EXISTS(SELECT * FROM Auth WHERE EntityTypeId = @RestActionEntityTypeId AND EntityId = @GetPersonAliasIdRestActionId)
                BEGIN
                    INSERT INTO [Auth] ([EntityTypeId], [EntityId], [Order], [Action], [AllowOrDeny], [SpecialRole], [GroupId], [Guid], [PersonAliasId]) 
                -- If there is already user defined security on GETapi/PersonAlias don't change it.
                IF NOT EXISTS(SELECT * FROM Auth WHERE EntityTypeId = @RestActionEntityTypeId AND EntityId = @GetPersonAliasRestActionId)
                BEGIN
                    INSERT INTO [Auth] ([EntityTypeId], [EntityId], [Order], [Action], [AllowOrDeny], [SpecialRole], [GroupId], [Guid], [PersonAliasId]) 
                END

                -- If there is already user defined security on GETapi/Groups/GroupTypeCheckinConfiguration/{groupTypeGuid} don't change it.
                IF NOT EXISTS(SELECT * FROM Auth WHERE EntityTypeId = @RestActionEntityTypeId AND EntityId = @GetGroupTypeCheckinConfigurationId)
                BEGIN
                    INSERT INTO [Auth] ([EntityTypeId], [EntityId], [Order], [Action], [AllowOrDeny], [SpecialRole], [GroupId], [Guid], [PersonAliasId]) 
                IF NOT EXISTS(SELECT * FROM Auth WHERE EntityTypeId = @RestActionEntityTypeId AND EntityId = @PutAddAttendanceId)
                BEGIN
                    INSERT INTO [Auth] ([EntityTypeId], [EntityId], [Order], [Action], [AllowOrDeny], [SpecialRole], [GroupId], [Guid], [PersonAliasId]) 
                IF NOT EXISTS(SELECT * FROM Auth WHERE EntityTypeId = @RestActionEntityTypeId AND EntityId = @PostAttendancesImportId)
                BEGIN
                    INSERT INTO [Auth] ([EntityTypeId], [EntityId], [Order], [Action], [AllowOrDeny], [SpecialRole], [GroupId], [Guid], [PersonAliasId]) 
                END" );
        }
    }
}