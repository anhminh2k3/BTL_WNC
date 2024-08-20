//Xu ly tìm kiem
document.getElementById('searchInput').addEventListener('input', function () {
    var searchValue = this.value.toLowerCase();
    var userCards = document.querySelectorAll('.user-card');

    userCards.forEach(function (card) {
        var name = card.querySelector('.user-info h3').textContent.toLowerCase();

        if (name.includes(searchValue)) {
            card.style.display = '';
        } else {
            card.style.display = 'none';
        }
    });
});

//delete
function confirmDeletion() {
    return confirm("Bạn có chắc chắn muốn xóa người dùng này không?");
}