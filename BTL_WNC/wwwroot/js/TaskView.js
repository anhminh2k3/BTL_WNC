//Xu ly tìm kiem
document.getElementById('taskSearch').addEventListener('input', function () {
    var searchValue = this.value.toLowerCase();
    var userCards = document.querySelectorAll('.card-custom');

    userCards.forEach(function (card) {
        var name = card.querySelector('.task-name').textContent.toLowerCase();

        if (name.includes(searchValue)) {
            card.style.display = '';
        } else {
            card.style.display = 'none';
        }
    });
});

//Xoa
function openDeleteModal(taskId) {
    document.getElementById('deleteTaskId').value = taskId;
    var deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));
    deleteModal.show();
}

//kéo thả task 
document.addEventListener('DOMContentLoaded', function () {
    var doingTasks = document.getElementById('doingTasks');
    var doneTasks = document.getElementById('doneTasks');

    new Sortable(doingTasks, {
        group: 'tasks',
        animation: 150,
        onEnd: function (evt) {
            var itemEl = evt.item;
            // Check which column the task is in now
            if (itemEl.parentNode.id === 'doneTasks') {
                updateTaskStatus(itemEl, 'Done');
            } else {
                updateTaskStatus(itemEl, 'Doing');
            }
        }
    });

    new Sortable(doneTasks, {
        group: 'tasks',
        animation: 150,
        onEnd: function (evt) {
            var itemEl = evt.item;
            // Check which column the task is in now
            if (itemEl.parentNode.id === 'doingTasks') {
                updateTaskStatus(itemEl, 'Doing');
            } else {
                updateTaskStatus(itemEl, 'Done');
            }
        }
    });

    function updateTaskStatus(taskElement, newStatus) {
        var taskId = taskElement.getAttribute('data-task-id');
        var token = document.querySelector('input[name="__RequestVerificationToken"]').value;

        console.log('updateTaskStatus called');
        console.log('taskId:', taskId);
        console.log('newStatus:', newStatus);

        fetch('/Task/UpdateStatus', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            },
            body: JSON.stringify({
                TaskId: taskId,
                NewStatus: newStatus
            })
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    console.log('Task status updated successfully');
                } else {
                    console.error('Failed to update task status:', data.message);
                }
            })
            .catch(error => {
                console.error('Error:', error);
            });
    }
});