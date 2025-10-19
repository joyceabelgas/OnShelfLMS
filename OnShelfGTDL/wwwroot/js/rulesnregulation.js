$(document).ready(function () {
    loadRules();

    // Collapse setup (only toggle once)
    $('#rulesCollapse').collapse({ toggle: false });

    $('#rulesCollapse').on('show.bs.collapse', function () {
        console.log('Collapse is shown');
    });

    $('#rulesCollapse').on('hide.bs.collapse', function () {
        console.log('Collapse is hidden');
    });

    $('.card-header').off('click').on('click', function () {
        $('#rulesCollapse').collapse('toggle');
    });

    $('#addRuleForm').submit(function (e) {
        e.preventDefault();

        const ruleId = $('#RuleId').val();

        var formData = {
            Id: ruleId,
            RuleTitle: $('input[name="RuleTitle"]').val(),
            Description1: $('input[name="Description1"]').val(),
            Description2: $('input[name="Description2"]').val(),
            Description3: $('input[name="Description3"]').val(),
            Description4: $('input[name="Description4"]').val()
        };

        const isEdit = ruleId !== "";

        $.ajax({
            url: isEdit ? '/RulesNRegulation/UpdateRule' : '/RulesNRegulation/AddRule',
            method: 'POST',
            data: formData,
            success: function () {
                showCustomAlert(isEdit ? 'Rule updated successfully!' : 'Rule saved successfully!', "success");
                $('#addRuleForm')[0].reset();
                $('#RuleId').val(""); // reset edit mode
                loadRules();
            },
            error: function () {
                alert(isEdit ? 'Failed to update rule.' : 'Failed to save rule.');
            }
        });
    });
});


function loadRules() {
    $.ajax({
        url: '/RulesNRegulation/GetRules', // adjust to your controller route
        method: 'GET',
        success: function (data) {
            var container = $('#rulesList');
            container.empty();

            if (data.length === 0) {
                container.append('<li class="list-group-item">No rules available.</li>');
                return;
            }

            $.each(data, function (i, rule) {
                let html = `
                    <li class="list-group-item">
                        <div class="fw-bold">${rule.ruleTitle}</div>
                        ${rule.description1 ? `<p class="mb-1 px-2">${rule.description1}</p>` : ''}
                        ${rule.description2 ? `<p class="mb-1 px-2">${rule.description2}</p>` : ''}
                        ${rule.description3 ? `<p class="mb-1 px-2">${rule.description3}</p>` : ''}
                        ${rule.description4 ? `<p class="mb-1 px-2">${rule.description4}</p>` : ''}

                        <div class="text-end">
                            <button class="btn btn-sm btn-primary me-2 btn-edit" data-id="${rule.id}">
                                <img src="/images/edit.png" alt="Edit" width="16" height="16">
                            </button>
                            <button class="btn btn-sm btn-danger btn-delete" data-id="${rule.id}">
                                <img src="/images/delete.png" alt="Delete" width="16" height="16">
                            </button>
                        </div>
                    </li>`;
                container.append(html);
            });
        },
        error: function () {
            $('#rulesList').html('<li class="list-group-item text-danger">Error loading rules.</li>');
        }
    });
}

$(document).on('click', '.btn-delete', function () {
    const ruleId = $(this).data('id');

    showConfirmModal("Are you sure you want to delete this rule?", function () {
        $.ajax({
            url: '/RulesNRegulation/Delete',
            method: 'POST',
            data: { id: ruleId },
            success: function () {
                showCustomAlert('Rule deleted successfully.', "success");
                loadRules(); // Reload the list of rules
            },
            error: function () {
                showCustomAlert('Failed to delete rule.', "danger");
            }
        });
    });
});



$(document).on('click', '.btn-edit', function () {
    const ruleId = $(this).data('id');

    $.ajax({
        url: '/RulesNRegulation/GetRuleById',
        method: 'GET',
        data: { id: ruleId },
        success: function (data) {
            // Populate form fields with the data
            $('input[name="RuleTitle"]').val(data.ruleTitle);
            $('input[name="Description1"]').val(data.description1);
            $('input[name="Description2"]').val(data.description2);
            $('input[name="Description3"]').val(data.description3);
            $('input[name="Description4"]').val(data.description4);

            // Add a hidden field for ID (if not already present)
            if ($('#addRuleForm input[name="Id"]').length === 0) {
                $('#addRuleForm').append(`<input type="hidden" name="Id" value="${data.id}" />`);
            } else {
                $('#addRuleForm input[name="Id"]').val(data.id);
            }

            // Change button text to "Update"
            $('#addRuleForm button[type="submit"]').text('Update');
        },
        error: function () {
            alert('Failed to load rule for editing.');
        }
    });
});



function showCustomAlert(message, type = "success", timeout = 3000) {
    const alertBox = document.getElementById("customAlert");

    if (alertBox) {
        // Set message and alert type (Bootstrap classes)
        document.getElementById("alertMessage").innerText = message;
        alertBox.className = `alert alert-${type} alert-dismissible fade show position-fixed top-0 start-50 translate-middle-x shadow`;

        // Show the alert
        alertBox.classList.remove("d-none");

        // Auto-hide after timeout
        const timer = setTimeout(() => {
            hideCustomAlert();
        }, timeout);

        // Ensure timeout clears if manually closed
        alertBox.setAttribute("data-timer", timer);
    }
}

// Reset and hide the alert completely
function hideCustomAlert() {
    const alertBox = document.getElementById("customAlert");
    if (alertBox) {
        alertBox.classList.add("d-none");
        alertBox.classList.remove("alert-success", "alert-danger", "alert-warning", "fade", "show");

        // Clear any existing timeout to avoid double hiding
        const timer = alertBox.getAttribute("data-timer");
        if (timer) clearTimeout(timer);
    }
}

let confirmCallback = null;

function showConfirmModal(message, yesCallback) {
    document.getElementById('confirmModalMessage').textContent = message;
    confirmCallback = yesCallback;
    var myModal = new bootstrap.Modal(document.getElementById('confirmModal'));
    myModal.show();
}

document.getElementById('confirmYesBtn').addEventListener('click', function () {
    if (typeof confirmCallback === 'function') {
        confirmCallback();
    }
    const modalEl = document.getElementById('confirmModal');
    bootstrap.Modal.getInstance(modalEl).hide();
    confirmCallback = null;
});