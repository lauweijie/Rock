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
using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Web.UI;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.CheckIn.Manager
{
    /// <summary>
    /// </summary>
    [DisplayName( "Attendance Detail" )]
    [Category( "Check-in > Manager" )]
    [Description( "Block to show details of a person's attendance" )]

    #region Block Attributes

    [AttributeField(
        "Attendance Attributes",
        Key = AttributeKey.AttendanceAttributes,
        Description = "The attendance attributes to show",
        EntityTypeGuid = Rock.SystemGuid.EntityType.ATTENDANCE,
        Order = 1 )]

    #endregion Block Attributes
    public partial class AttendanceDetail : RockBlock
    {

        #region Attribute Keys

        private static class AttributeKey
        {
            public const string AttendanceAttributes = "AttendanceAttributes";
        }

        #endregion Attribute Keys

        #region PageParameterKeys

        private static class PageParameterKey
        {
            public const string AttendanceGuid = "Attendance";
            public const string AttendanceId = "AttendanceId";
        }

        #endregion PageParameterKeys

        #region Fields

        // used for private variables

        #endregion

        #region Properties

        // used for public / protected properties

        #endregion

        #region Base Control Methods

        //  overrides of the base RockBlock methods (i.e. OnInit, OnLoad)

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            RockPage.AddCSSLink( "~/Styles/fluidbox.css" );
            RockPage.AddScriptLink( "~/Scripts/imagesloaded.min.js" );
            RockPage.AddScriptLink( "~/Scripts/jquery.fluidbox.min.js" );

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                ShowDetail( GetAttendanceGuid() );
            }
        }

        #endregion

        #region Events

        // handlers called by the controls on your block

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {

        }

        #endregion

        #region Methods

        private Guid? _attendanceGuid;

        /// <summary>
        /// Gets the attendance unique identifier based on what was passed into the page parameters
        /// </summary>
        private Guid? GetAttendanceGuid()
        {
            if ( _attendanceGuid.HasValue )
            {
                return _attendanceGuid.Value;
            }

            Guid? attendanceGuid = PageParameter( PageParameterKey.AttendanceGuid ).AsGuidOrNull();
            if ( attendanceGuid.HasValue )
            {
                return attendanceGuid.Value;
            }

            int? attendanceId = PageParameter( PageParameterKey.AttendanceId ).AsIntegerOrNull();
            if ( attendanceId.HasValue )
            {
                using ( var rockContext = new RockContext() )
                {
                    _attendanceGuid = new AttendanceService( rockContext ).GetGuid( attendanceId.Value );
                }
            }

            return _attendanceGuid;
        }

        private void ShowDetail( Guid? attendanceGuid )
        {
            if ( !attendanceGuid.HasValue )
            {
                return;
            }

            var rockContext = new RockContext();
            var attendanceService = new AttendanceService( rockContext );

            // fetch the attendance record and include all the details that we'll be displaying
            var attendance = attendanceService.Queryable()
                .Where( a => a.Guid == attendanceGuid.Value && a.PersonAliasId.HasValue )
                .Include( a => a.PersonAlias.Person )
                .Include( a => a.Occurrence.Group )
                .Include( a => a.Occurrence.Schedule )
                .Include( a => a.Occurrence.Location )
                .Include( a => a.AttendanceCode )
                .Include( a => a.CheckedOutByPersonAlias.Person )
                .Include( a => a.PresentByPersonAlias.Person )
                .AsNoTracking()
                .FirstOrDefault();

            if ( attendance == null )
            {
                return;
            }

            ShowPersonDetails( attendance );
            ShowAttendanceDetails( attendance );
        }

        private void ShowPersonDetails( Attendance attendance )
        {
            var person = attendance.PersonAlias.Person;

            lName.Text = person.FullName;

            string photoTag = Rock.Model.Person.GetPersonPhotoImageTag( person, 200, 200 );
            if ( person.PhotoId.HasValue )
            {
                lPhoto.Text = string.Format( "<div class='photo'><a href='{0}'>{1}</a></div>", person.PhotoUrl, photoTag );
            }
            else
            {
                lPhoto.Text = photoTag;
            }

            var campus = person.GetCampus();
            if ( campus != null )
            {
                hlCampus.Visible = true;
                hlCampus.Text = campus.Name;
            }
            else
            {
                hlCampus.Visible = false;
            }

            lEmail.Visible = !string.IsNullOrWhiteSpace( person.Email );
            lEmail.Text = string.Format( @"<div class=""text-truncate"">{0}</div>", person.GetEmailTag( ResolveRockUrl( "/" ), "text-color" ) );

            var phoneNumbers = person.PhoneNumbers.Where( p => !p.IsUnlisted ).ToList();
            rptrPhones.DataSource = phoneNumbers;
            rptrPhones.DataBind();
        }

        private void ShowAttendanceDetails( Attendance attendance )
        {
            var occurrence = attendance.Occurrence;

            var groupType = GroupTypeCache.Get( occurrence.Group.GroupTypeId );
            var groupPath = new GroupTypeService( new RockContext() ).GetAllCheckinAreaPaths().FirstOrDefault( a => a.GroupTypeId == occurrence.Group.GroupTypeId );

            lGroupName.Text = string.Format( "{0} > {1}", groupPath, occurrence.Group );
            lLocationName.Text = occurrence.Location.Name;
            if ( attendance.AttendanceCode != null )
            {
                lTag.Text = attendance.AttendanceCode.Code;
            }

            lScheduleName.Text = occurrence.Schedule.Name;

            var personAliasService = new PersonAliasService( new RockContext() );
            var checkInPersonAlias = attendance.CheckedInByPersonAliasId.HasValue ? personAliasService.Get( attendance.CheckedInByPersonAliasId.Value ) : null;

            SetCheckinPersonLabel( checkInPersonAlias, lCheckinByPerson );
            SetCheckinPersonLabel( attendance.PresentByPersonAlias, lPresentByPerson );
            SetCheckinPersonLabel( attendance.CheckedOutByPersonAlias, lCheckedOutByPerson );

            lCheckinTime.Text = attendance.StartDateTime.ToString();

            if ( attendance.PresentDateTime.HasValue )
            {
                lPresentTime.Visible = true;
                lPresentTime.Text = attendance.PresentDateTime.ToString();
            }
            else
            {
                lPresentTime.Visible = false;
            }

            if ( attendance.EndDateTime.HasValue )
            {
                lCheckedOutTime.Visible = true;
                lCheckedOutTime.Text = attendance.EndDateTime.ToString();
            }
            else
            {
                lCheckedOutTime.Visible = false;
            }

            if ( attendance.EndDateTime.HasValue )
            {
                lCheckedOutTime.Visible = true;
                lCheckedOutTime.Text = attendance.PresentDateTime.ToString();
            }
            else
            {
                lCheckedOutTime.Visible = false;
            }
        }

        /// <summary>
        /// Sets the checkin person label.
        /// </summary>
        /// <param name="personAlias">The person alias.</param>
        /// <param name="rockLiteral">The rock literal.</param>
        private static void SetCheckinPersonLabel( PersonAlias personAlias, RockLiteral rockLiteral )
        {
            if ( personAlias == null || personAlias.Person == null )
            {
                rockLiteral.Visible = false;
                return;
            }

            rockLiteral.Visible = true;

            var checkedInByPersonName = personAlias.Person.FullName;
            var checkedInByPersonPhone = personAlias.Person.GetPhoneNumber( Rock.SystemGuid.DefinedValue.PERSON_PHONE_TYPE_MOBILE.AsGuid() );
            rockLiteral.Text = string.Format( "{0} {1}", checkedInByPersonName, checkedInByPersonPhone );
        }

        #endregion

        protected void btnEditAttributes_Click( object sender, EventArgs e )
        {

        }

        protected void btnSaveAttributes_Click( object sender, EventArgs e )
        {

        }

        protected void btnCancelAttributes_Click( object sender, EventArgs e )
        {

        }

        protected void btnLaunchWorkflow_Click( object sender, EventArgs e )
        {

        }
    }
}