// TitanHelp Site JavaScript

// Auto-dismiss alerts after 5 seconds
document.addEventListener('DOMContentLoaded', function () {
    const alerts = document.querySelectorAll('.alert-dismissible');
    alerts.forEach(function (alert) {
        setTimeout(function () {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }, 5000);
    });
});

// Confirm delete actions
function confirmDelete(ticketName) {
    return confirm(`Are you sure you want to delete ticket "${ticketName}"? This action cannot be undone.`);
}

// Character counter utility
function updateCharacterCount(input, counterId, maxLength) {
    const counter = document.getElementById(counterId);
    if (!counter) return;

    const length = input.value.length;
    counter.textContent = `${length} / ${maxLength} characters`;

    if (length > maxLength * 0.9) {
        counter.classList.add('text-danger');
        counter.classList.remove('text-muted');
    } else {
        counter.classList.add('text-muted');
        counter.classList.remove('text-danger');
    }
}

// Form validation helper
function validateForm(formId) {
    const form = document.getElementById(formId);
    if (!form) return true;

    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
        form.classList.add('was-validated');
        return false;
    }

    return true;
}

// Smooth scroll to top
function scrollToTop() {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
}

// Show/hide loading spinner
function showLoading(show = true) {
    const spinner = document.getElementById('loadingSpinner');
    if (spinner) {
        spinner.style.display = show ? 'block' : 'none';
    }
}

// Format date helper
function formatDate(dateString) {
    const date = new Date(dateString);
    const options = {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    };

    return date.toLocaleDateString('en-US', options);
}

// Priority badge color helper
function getPriorityBadgeClass(priority) {
    switch (priority.toLowerCase()) {
        case 'high':
            return 'bg-danger';
        case 'medium':
            return 'bg-warning text-dark';
        case 'low':
            return 'bg-info text-dark';
        default:
            return 'bg-secondary';
    }
}

// Status badge color helper
function getStatusBadgeClass(status) {
    switch (status.toLowerCase()) {
        case 'open':
            return 'bg-success';
        case 'in progress':
            return 'bg-primary';
        case 'closed':
            return 'bg-secondary';
        default:
            return 'bg-secondary';
    }
}
