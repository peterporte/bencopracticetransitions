// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function BindWindowUnloadEventListener() {
    window.addEventListener('beforeunload', NavigateAway);
}


function NavigateAway(e) {
    if (submitted) {
        return;
    }

    if (FormFieldsHasData() === true) {
        e.preventDefault(true);
        e.returnValue = "Are you sure you want to leave?";
        return;
    } 
}

function FormFieldsHasData() {
    var formFieldsHasData = false;
    $(".window-unload-bound").each(function () {
        if ($(this).is(":checkbox") || $(this).is(":radio")) {
            if ($(this).is(":checked")) {
                formFieldsHasData = true;
            }
        } else if ($(this).val() !== "") {
            formFieldsHasData = true;
        }
    });
    return formFieldsHasData;
}



/*
 * Hides or shows a field based on the value of another field
 * @param {any} fieldToHideShow - The name of the field to hide or show. Note the field must be wrapped in a div with the
 *                                same name as the field to hide/show with "Panel" appended to the end of the name. i.e. if the
 *                                field to hide/show is named text1 then it must reside inside a div called text1Panel.
  * @param {any} fieldToTest    - The name of the field whose value will be compared
 * @param {any} valueToHideOn   - The value that if contained in fieldToTest will show the field specified by fieldToHideShow. If
 *                                fieldToTest contains any other value, fieldTohideShow will be hidden
 */
function SetDependantFieldVisibility(fieldToHideShow, fieldToTest, valueToShowOn) {
    var panelToHideShow = fieldToHideShow + "Panel";

    if ($("#" + fieldToTest).val() !== valueToShowOn) {
        $("#" + panelToHideShow).hide();
    }
    $("#" + fieldToTest).change(function () {
        if ($("#" + fieldToTest).val() === valueToShowOn) {
            $("#" + panelToHideShow).show();
        } else {
            $("#" + panelToHideShow).hide();
            $("#" + fieldToHideShow).val("").removeClass("input-validation-error");
            $("#" + panelToHideShow + " span").val("").addClass("field-validation-valid");
        }
    });
}



function SetHowDidYouHearAboutUsDetail() {

    if (HowDidYouHearAboutUsOtherDetailsNotRequired($("#HowDidYouHearAboutUs option:selected").text())) {
        $("#howDidYouHearAboutUsDetailPanel").hide();
    }
    $("#HowDidYouHearAboutUs").change(function () {
        if (HowDidYouHearAboutUsOtherDetailsNotRequired($("#HowDidYouHearAboutUs option:selected").text())) {
            $("#howDidYouHearAboutUsDetailPanel").hide();
            $("#howDidYouHearAboutUsDetail").val("").removeClass("input-validation-error");
            $("#howDidYouHearAboutUsDetailPanel span").val("").addClass("field-validation-valid");

        } else {
            $("#howDidYouHearAboutUsDetailPanel").show();
            SetHowDisYouHearAboutUsDetailLabel();
            SetHelpText();
            }
    });
}

function HowDidYouHearAboutUsOtherDetailsRequired(value) {
    return !HowDidYouHearAboutUsOtherDetailsNotRequired(value);
}

function HowDidYouHearAboutUsOtherDetailsNotRequired(value) {

    return value === "" || value === "Benco Website" || value === "Benco Customer Service";
}

function SetHelpText() {

    switch ($("#HowDidYouHearAboutUs").val()) {
        case "InternetSearch":
            $("#HowDidYouHearAboutUsDetail").attr("placeholder", "Google");
            break;           
        case "Other":
            $("#HowDidYouHearAboutUsDetail").attr("placeholder", "Please explain.");
            break;
        case "Podcast":
            $("#HowDidYouHearAboutUsDetail").attr("placeholder", "Gentle Dental");
            break;
        default:
            $("#HowDidYouHearAboutUsDetail").attr("placeholder", "John Doe");
            break;
    }
}

function SetHowDisYouHearAboutUsDetailLabel() {

    switch ($("#HowDidYouHearAboutUs").val()) {
        case "InternetSearch":
            $("#howDidYouHearAboutUsDetailLabel").text("Search Engine (i.e. Google, Bing, etc.)");
            break;
        case "Other":
            ($("#howDidYouHearAboutUsDetailLabel").text("Other details"));
            break;
        case "Podcast":
            $("#howDidYouHearAboutUsDetailLabel").text($("#HowDidYouHearAboutUs option:selected").text() + "'s Name");
            break;
        default:
            $("#howDidYouHearAboutUsDetailLabel").text($("#HowDidYouHearAboutUs option:selected").text() + "'s Name");
            break;
    }
}

function addAlertInfo(placeholderId, message) {
    $("#" + placeholderId).html('<div class="alert alert-info alert-dismissable"><button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button><p>' +
        message + "</p></div>");
}