$(function () {
    GetListskill();
})

function AddClick() {
    $("#Name").val("");
    $("#Detail").val("");
    $("#ID").val("0");
    $("#modalTitle").html("Thêm kĩ năng");
    for (var i = 1; i < 4; i++) {
        $('#level-' + i).val('');
    }
    $("#modalAddSkill").modal();
}
function EditClick(ID, name, detail, levels) {
    for (var i = 1; i < 4; i++) {
        $('#level-' + i).val('');
    }
    $("#Name").val(name);
    $("#Detail").val(detail);
    $("#ID").val(ID);
    $("#modalTitle").html("Sửa kĩ năng");
    var levelArr = levels.split(',');
    levelArr.forEach(function (item, index) {
        $('#level-' + (index + 1)).val(item);
    });
    $("#modalAddSkill").modal();
}

var selected_skill;
function DeleteClick(context) {
    selected_skill = $(context).attr("name");
    $("#alertWarning").modal();
}

function AddSkill() {
    var ID = $("#ID").val();
    var Name = $("#Name").val();
    var Detail = $("#Detail").val();
    var levelI = $('#level-1').val();
    var levelII = $('#level-2').val();
    var levelIII = $('#level-3').val();
    var levels = levelI + ',' + levelII + ',' + levelIII;
    $.ajax({
        url: "/Skill/AddSkill",
        type: "POST",
        data: { ID: ID, Name: Name, Detail: Detail, levels : levels},
        success: function () {
            GetListskill();
        },
        error: function () {
        }
    });
    $('#modalAddSkill').modal('toggle');
}
function DeleteSkill() {
    $.ajax({
        url: "/Skill/DeleteSkill",
        type:"POST",
        data: { ID: selected_skill },
        success: function () {
            GetListskill();
        },
        error: function () {
        }
    });
    $('#alertWarning').modal('toggle');
}
function GetListskill() {
    $(".loading").show();
    $.ajax({
        url: "/Skill/GetListSkill",
        success: function (response) {
            $('#list_skill').html(response.body);
            $(".loading").hide();
        },
        error: function (e) {
            console.log(e);
            $(".loading").hide();
        }
    });
}
