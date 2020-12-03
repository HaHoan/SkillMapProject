$(function () {
    GetListskill();
})

function AddClick() {
    $("#Name").val("");
    $("#Dept").val("");
    $("#Detail").val("");
    $("#ID").val("0");
    $("#modalTitle").html("Thêm kĩ năng");
    $("#modalAddSkill").modal();
}
function EditClick(context) {
    var ID = $(context).attr("name");
    var name = $(context).parents("tr").find(".nametb").text();
    var dept = $(context).parents("tr").find(".depttb").text();
    var detail = $(context).parents("tr").find(".detailtb").text();
    $("#Name").val(name);
    $("#Dept").val(dept);
    $("#Detail").val(detail);
    $("#ID").val(ID);
    $("#modalTitle").html("Sửa kĩ năng");

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
    var Dept = $("#Dept").val();
    var Detail = $("#Detail").val();
    $.ajax({
        url: "/Home/AddSkill",
        type: "POST",
        data: { ID: ID, Name: Name, Detail: Detail, Dept : Dept },
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
        url: "/Home/DeleteSkill",
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
        url: "/Home/GetListSkill",
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
