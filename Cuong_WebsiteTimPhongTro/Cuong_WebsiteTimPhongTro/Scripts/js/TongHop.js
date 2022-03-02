/* snip2_button */
$(".hover").mouseleave(
    function () {
        $(this).removeClass("hover");
    }
);

/* Chỉnh chữ thẻ h1,h2,... */
// Created for an Articles on: Được tạo cho một Bài báo trên
// https://www.html5andbeyond.com/bubbling-text-effect-no-canvas-required/

jQuery(document).ready(function ($) {

    // Xác định một mảng trống cho các vị trí hiệu ứng. Điều này sẽ được điền dựa trên chiều rộng của tiêu đề.
    var bArray = [];
    //Xác định mảng kích thước, mảng này sẽ được sử dụng để thay đổi kích thước bong bóng
    var sArray = [4, 6, 8, 10];

    // Đẩy các giá trị chiều rộng tiêu đề vào bArray
    for (var i = 0; i < $('.bubbles').width(); i++) {
        bArray.push(i);
    }

    // Hàm chọn phần tử mảng ngẫu nhiên
    // Được sử dụng trong setInterval một vài lần
    function randomValue(arr) {
        return arr[Math.floor(Math.random() * arr.length)];
    }

    // Hàm setInterval được sử dụng để tạo bong bóng mới sau mỗi 350 mili giây
    setInterval(function () {

        // Nhận một kích thước ngẫu nhiên, được xác định là biến để nó có thể được sử dụng cho cả chiều rộng và chiều cao
        var size = randomValue(sArray);
        // Bong bóng mới được thêm vào div với kích thước và vị trí bên trái của nó được đặt nội tuyến
        // Giá trị bên trái được đặt thông qua việc nhận một giá trị ngẫu nhiên từ bArray
        $('.bubbles').append('<div class="individual-bubble" style="left: ' + randomValue(bArray) + 'px; width: ' + size + 'px; height:' + size + 'px;"></div>');

        // Tạo hoạt ảnh cho từng bong bóng lên trên cùng (100% dưới cùng) và giảm độ mờ khi nó di chuyển
        // Chức năng gọi lại được sử dụng để xóa các hoạt ảnh đã hoàn thành khỏi trang
        $('.individual-bubble').animate({
            'bottom': '100%',
            'opacity': '-=0.7'
        }, 3000, function () {
            $(this).remove()
        }
        );


    }, 250);

});