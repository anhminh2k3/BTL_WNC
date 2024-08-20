
document.querySelectorAll('#closeAddModalButton).forEach(function (button) {
    button.addEventListener('click', function () {
    this.closest('.modal').style.display = 'none';
    });
});
