document.addEventListener("DOMContentLoaded", function () {
    var dueDateInput = document.getElementById("dueDate");
    var updateAtInput = document.getElementById("updateAt");

    // Set the update date to today
    var today = new Date().toISOString().split('T')[0];
    updateAtInput.value = today;

    // Set the minimum date for the due date to today + 1
    var tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    var minDueDate = tomorrow.toISOString().split('T')[0];
    dueDateInput.setAttribute('min', minDueDate);

    // Add an event listener to validate the due date
    dueDateInput.addEventListener("change", function () {
        var selectedDate = new Date(dueDateInput.value);
        var updateAtDate = new Date(updateAtInput.value);

        // Ensure Due Date is strictly greater than UpdateAt date
        if (selectedDate <= updateAtDate) {
            alert("Due Date must be after the UpdateAt date.");
            dueDateInput.value = "";
        }
    });
});