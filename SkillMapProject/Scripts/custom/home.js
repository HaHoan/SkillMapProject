const SUCCESS = 1;
const ERROR = 2;
$(function () {
    GetSkillOfStaffListWithSearch();
    getDate();
    addLevelForSkill($('#skill_selected').val());
    $('#skill_selected').change(function () {
        addLevelForSkill($('#skill_selected').val());
    });
    $('#capdo_selected').change(function () {
        changeLevelByCapDo();
    });
    $('#dept').change(function () {
        GetSkillOfStaffListWithDept();
    });
})
function changeLevelByCapDo() {
    var capdo = $('#capdo_selected').val();
    $('#levels_selected').val(capdo);
}
function addLevelForSkill(levels) {
    $('#levels_selected').empty();
    $('#capdo_selected').empty();

    var arr = levels.split(',');
    arr.forEach(function (item, index) {
        if (item != '') {
            $('#levels_selected').append('<option value = "' + index + '" name = "' + item + '">' + item + '</option>');
            $('#capdo_selected').append('<option value = "' + index + '"> Cấp Độ ' + (index + 1) + '</option>');
        }

    });
    changeLevelByCapDo();
}
function GetSkillOfStaffListWithSearch() {
    var search = $('#search').val();
    $(".loading").show();
    $.ajax({
        url: "/Home/GetSkillOfStaffListWithSearch",
        data: { search: search },
        success: function (response) {
            $('#list_skill_of_staff').html(response.body);
            $(".loading").hide();
        },
        error: function (e) {
            console.log(e);
            $(".loading").hide();
        }
    });
}
function GetSkillOfStaffListWithDept() {
    var dept = $('#dept').find('option:selected').attr("name");
    $(".loading").show();
    $.ajax({
        url: "/Home/GetSkillOfStaffListWithDept",
        data: { dept: dept },
        success: function (response) {
            $('#list_skill_of_staff').html(response.body);
            $(".loading").hide();
        },
        error: function (e) {
            console.log(e);
            $(".loading").hide();
        }
    });
    
}

var selectedUser = "";
function addSkillForStaff(userId) {
    selectedUser = userId;
}
$("#search").on("keydown", function (event) {
    if (event.which == 13) {
        GetSkillOfStaffListWithSearch();
    }

});
function submitSkillForStaff() {
    var CapDo = $('#levels_selected').find('option:selected').attr("name");
    var NgayCap = $('#NgayCap').val();
    var NgayThiXacNhan = $('#NgayThiXacNhan').val();
    var SkillID = $('#skill_selected').find('option:selected').attr("name");
    $.ajax({
        url: "/Home/ThemKiNangChoNV",
        type: "POST",
        data: {
            userID: selectedUser,
            CapDo: CapDo,
            NgayCap: NgayCap,
            NgayThiXacNhan: NgayThiXacNhan,
            SkillID: SkillID
        },
        success: function (response) {
            if (response.body == 'OK') {
                GetSkillOfStaffListWithSearch();
                $('#modalAddSkillForStaff').modal('hide');
            } else if (response.body == "NG") {
                alert("Có lỗi xảy ra!");
            } else {
                alert(response.body);
            }
        },
        error: function (e) {

        }
    });
}

function UpdateNewStaff() {
    $(".loading").show();
    $.ajax({
        url: "/Home/UpdateNewStaff",
        type: "GET",
        success: function (response) {
            if (response.body == "NG") {
                alert("Có lỗi xảy ra!");
            } else {
                alert(response.body);
            }
            $(".loading").hide();
        },
        error: function (e) {
            alert(e);
        }
    });
}

function exportToExcel() {
    var skillID = $("#list_skill option:selected").val();
    window.location.href = 'Home/Export?SkillID=' + skillID;
   
}