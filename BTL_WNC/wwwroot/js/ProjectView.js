document.getElementById('searchInput').addEventListener('input', function () {
    var searchValue = this.value.toLowerCase();
    var projectCards = document.querySelectorAll('.project-card');

    projectCards.forEach(function (card) {
        var name = card.querySelector('h4').textContent.toLowerCase();

        if (name.includes(searchValue)) {
            card.style.display = '';
        } else {
            card.style.display = 'none';
        }
    });
});

document.querySelectorAll('.edit-project-btn').forEach(function (button) {
    button.addEventListener('click', function (e) {
        e.preventDefault();
        var url = this.getAttribute('data-url');
        loadEditModal(url);
    });
});

document.getElementById('saveChangesButton').addEventListener('click', function () {
    var form = document.getElementById('editProjectForm');
    var formData = new FormData(form);

    fetch(form.getAttribute('action'), {
        method: 'POST',
        body: formData
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                document.getElementById('editProjectModal').style.display = 'none';
                location.reload();
            } else {
                alert('Error: ' + data.errors.join(', '));
            }
        })
        .catch(() => alert('Failed to save changes.'));
});

document.querySelectorAll('#closeAddModalButton, #closeEditModalButton').forEach(function (button) {
    button.addEventListener('click', function () {
        this.closest('.modal').style.display = 'none';
    });
});

document.getElementById('addProjectButton').addEventListener('click', function () {
    document.getElementById('addProjectModal').style.display = 'flex';
});

function loadEditModal(url) {
    fetch(url)
        .then(response => response.text())
        .then(data => {
            document.querySelector('#editProjectModal .modal-body').innerHTML = data;
            document.getElementById('editProjectModal').style.display = 'flex';
        })
        .catch(() => alert('Failed to load data.'));
}

function confirmDelete(projectId) {
    if (confirm('Bạn chắc chắn muốn xóa Project này chứ?')) {
        $.ajax({
            url: /*'@Url.Action("DeleteProject", "Project")',*/ '/Project/DeleteProject',
            type: 'POST',
            data: { id: projectId },
            success: function (result) {
                if (result.success) {
                    location.reload(); // Tải lại trang sau khi xóa thành công
                } else {
                    alert('Error!!!');
                }
            }
        });
    }
    console.log('Delete Project URL:', '@Url.Action("DeleteProject", "Project")');

    return false;
}
