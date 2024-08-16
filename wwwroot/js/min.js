
function showAddItemModal() {

    $("#Add-Item").modal("show");
}


function showDamagedProductsModal() {

    $("#Add-Damaged-Item").modal("show");
}

function showProductsDeatilsModal() {

    $("#Add-Deatils-Item").modal("show");
}

function showEditModel(id) {
    console.log(id);
    $("#ُEdit-Item").modal('show');
    // تصحيح مسار الـ URL بإضافة `=` بعد `id`
    let url = "/ProductsDetails/EditProductsDetails";
    let productId = document.getElementById("id");
    let price = document.getElementById("price");
    let qty = document.getElementById("qty");
    let color = document.getElementById("color");
    let imgPreview = document.getElementById("imgPreview");

    $.ajax({
        url: url,
        type: "POST",
        data: {
            id: id,
        },
        success: function (res) {
            console.table(res); // عرض البيانات في جدول
            // تعيين القيم للعناصر
            productId.value = res.productId;
            price.value = res.price;
            qty.value = res.qty;
            color.value = res.color;
            imgPreview.src = res.images;
        },
        error: function (xhr, status, error) {
            console.error("An error occurred: " + error); // التعامل مع الأخطاء
        }
    });


}

function showProductsEditModel(id) {
    console.log(id);
    $("#ُEdit-Product").modal('show');
    let url = "/AddNewItem/Edit";
    let productId = document.getElementById("id");
    let name = document.getElementById("update-name");
    let description = document.getElementById("update-description");

    $.ajax({
        url: url,
        type: "POST",
        data: {
            id: id,
        },
        success: function (res) {
            console.table(res); // عرض البيانات في جدول
            // تعيين القيم للعناصر
            productId.value = res.id;
            name.value = res.name;
            description.value = res.description;
        },
        error: function (xhr, status, error) {
            console.error("An error occurred: " + error); // التعامل مع الأخطاء
        }
    });


}