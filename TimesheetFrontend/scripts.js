// Configuration
const BASE_URL = 'https://localhost:7294/api';
// UI Elements
const messageContainer = document.getElementById('message-container');
const loginForm = document.getElementById('loginForm');
const registerForm = document.getElementById('registerForm');
const timesheetForm = document.getElementById('timesheetForm');
const timesheetTableBody = document.getElementById('timesheetTableBody');
const loginTab = document.getElementById('login-tab');
const registerTab = document.getElementById('register-tab');
const timesheetTab = document.getElementById('timesheet-tab');
const logoutBtn = document.getElementById('logout-btn');
const submitBtn = document.getElementById('submitBtn');
const cancelBtn = document.getElementById('cancelBtn');

// State
let employeeId = null;
let token = null;

// --- Utility Functions ---

function showMessage(message, type = 'success') {
    messageContainer.textContent = message;
    messageContainer.className = `alert mt-3 alert-${type}`;
    messageContainer.classList.remove('d-none');
    setTimeout(() => {
        messageContainer.classList.add('d-none');
    }, 5000);
}

function toggleAuthUI(isLoggedIn) {
    if (isLoggedIn) {
        loginTab.style.display = 'none';
        registerTab.style.display = 'none';
        timesheetTab.style.display = 'block';
        logoutBtn.style.display = 'block';
        new bootstrap.Tab(timesheetTab).show();
    } else {
        loginTab.style.display = 'block';
        registerTab.style.display = 'block';
        timesheetTab.style.display = 'none';
        logoutBtn.style.display = 'none';
        new bootstrap.Tab(loginTab).show();
    }
}

function getAuthHeaders() {
    return {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
    };
}

// --- API Interactions ---

async function registerUser(fullName, email, password, confirmPassword) {
    try {
        const response = await fetch(`${BASE_URL}/employees/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ fullName, email, password, confirmPassword })
        });
        const data = await response.json();
        if (response.ok) {
            showMessage(data.message, 'success');
            registerForm.reset();
        } else {
            showMessage(data.message || 'Registration failed.', 'danger');
        }
    } catch (error) {
        showMessage('An error occurred during registration.', 'danger');
    }
}

async function loginUser(email, password) {
    try {
        const response = await fetch(`${BASE_URL}/employees/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password })
        });
        const data = await response.json();
        if (response.ok) {
            // Store the token and employeeId
            token = data.token; // Assuming the backend returns a token
            employeeId = data.employeeId;
            localStorage.setItem('employeeId', employeeId);
            localStorage.setItem('token', token);
            showMessage(data.message, 'success');
            loginForm.reset();
            toggleAuthUI(true);
            await fetchTimesheetEntries();
        } else {
            showMessage(data.message || 'Login failed.', 'danger');
        }
    } catch (error) {
        showMessage('An error occurred during login.', 'danger');
    }
}

async function fetchTimesheetEntries() {
    try {
        const response = await fetch(`${BASE_URL}/timesheets`, {
            method: 'GET',
            headers: getAuthHeaders()
        });
        if (response.ok) {
            const entries = await response.json();
            renderTimesheetEntries(entries);
        } else {
            showMessage('Failed to load timesheet entries.', 'danger');
        }
    } catch (error) {
        showMessage('An error occurred while fetching timesheet entries.', 'danger');
    }
}

async function addTimesheetEntry(entry) {
    try {
        const response = await fetch(`${BASE_URL}/timesheets`, {
            method: 'POST',
            headers: getAuthHeaders(),
            body: JSON.stringify(entry)
        });
        if (response.ok) {
            showMessage('Entry added successfully!', 'success');
            timesheetForm.reset();
            await fetchTimesheetEntries();
        } else {
            showMessage('Failed to add entry.', 'danger');
        }
    } catch (error) {
        showMessage('An error occurred while adding the entry.', 'danger');
    }
}

async function updateTimesheetEntry(id, entry) {
    try {
        const response = await fetch(`${BASE_URL}/timesheets/${id}`, {
            method: 'PUT',
            headers: getAuthHeaders(),
            body: JSON.stringify(entry)
        });
        if (response.ok) {
            showMessage('Entry updated successfully!', 'success');
            timesheetForm.reset();
            submitBtn.textContent = 'Add Entry';
            cancelBtn.style.display = 'none';
            document.getElementById('entryId').value = '';
            await fetchTimesheetEntries();
        } else {
            showMessage('Failed to update entry.', 'danger');
        }
    } catch (error) {
        showMessage('An error occurred while updating the entry.', 'danger');
    }
}

async function deleteTimesheetEntry(id) {
    try {
        const response = await fetch(`${BASE_URL}/timesheets/${id}`, {
            method: 'DELETE',
            headers: getAuthHeaders()
        });
        if (response.ok) {
            showMessage('Entry deleted successfully!', 'success');
            await fetchTimesheetEntries();
        } else {
            showMessage('Failed to delete entry.', 'danger');
        }
    } catch (error) {
        showMessage('An error occurred while deleting the entry.', 'danger');
    }
}

// --- UI Rendering ---

function renderTimesheetEntries(entries) {
    timesheetTableBody.innerHTML = '';
    entries.forEach(entry => {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${new Date(entry.date).toLocaleDateString()}</td>
            <td>${entry.projectName}</td>
            <td>${entry.hoursWorked}</td>
            <td>${entry.description}</td>
            <td>
                <button class="btn btn-sm btn-warning edit-btn" data-id="${entry.id}">Edit</button>
                <button class="btn btn-sm btn-danger delete-btn" data-id="${entry.id}">Delete</button>
            </td>
        `;
        timesheetTableBody.appendChild(row);
    });
}

// --- Event Listeners ---

registerForm.addEventListener('submit', (e) => {
    e.preventDefault();
    const fullName = document.getElementById('registerFullName').value;
    const email = document.getElementById('registerEmail').value;
    const password = document.getElementById('registerPassword').value;
    const confirmPassword = document.getElementById('registerConfirmPassword').value;
    registerUser(fullName, email, password, confirmPassword);
});

loginForm.addEventListener('submit', (e) => {
    e.preventDefault();
    const email = document.getElementById('loginEmail').value;
    const password = document.getElementById('loginPassword').value;
    loginUser(email, password);
});

logoutBtn.addEventListener('click', () => {
    token = null;
    employeeId = null;
    localStorage.removeItem('employeeId');
    localStorage.removeItem('token');
    showMessage('You have been logged out.', 'info');
    toggleAuthUI(false);
});

timesheetForm.addEventListener('submit', (e) => {
    e.preventDefault();
    const id = document.getElementById('entryId').value;
    const date = document.getElementById('date').value;
    const projectName = document.getElementById('projectName').value;
    const hoursWorked = parseFloat(document.getElementById('hoursWorked').value);
    const description = document.getElementById('description').value;
    const entry = { employeeId: employeeId, date, projectName, hoursWorked, description };

    if (id) {
        updateTimesheetEntry(id, entry);
    } else {
        addTimesheetEntry(entry);
    }
});

timesheetTableBody.addEventListener('click', (e) => {
    if (e.target.classList.contains('delete-btn')) {
        const id = e.target.dataset.id;
        if (confirm('Are you sure you want to delete this entry?')) {
            deleteTimesheetEntry(id);
        }
    }
    if (e.target.classList.contains('edit-btn')) {
        const id = e.target.dataset.id;
        const row = e.target.closest('tr');
        document.getElementById('entryId').value = id;
        document.getElementById('date').value = new Date(row.cells[0].textContent).toISOString().substring(0, 10);
        document.getElementById('projectName').value = row.cells[1].textContent;
        document.getElementById('hoursWorked').value = row.cells[2].textContent;
        document.getElementById('description').value = row.cells[3].textContent;
        submitBtn.textContent = 'Update Entry';
        cancelBtn.style.display = 'block';
    }
});

cancelBtn.addEventListener('click', () => {
    timesheetForm.reset();
    submitBtn.textContent = 'Add Entry';
    cancelBtn.style.display = 'none';
    document.getElementById('entryId').value = '';
});

// Initial check on page load
document.addEventListener('DOMContentLoaded', () => {
    employeeId = localStorage.getItem('employeeId');
    token = localStorage.getItem('token');
    if (token && employeeId) {
        toggleAuthUI(true);
        fetchTimesheetEntries();
    } else {
        toggleAuthUI(false);
    }
});
