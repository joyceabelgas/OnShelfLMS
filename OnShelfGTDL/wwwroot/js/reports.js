const columnHeaderTemplates = {
    borrowed: ['Title', 'Borrower', 'BorrowedDate', 'DueDate', 'Status'],
    overdue: ['Title', 'Borrower', 'DueDate', 'DaysOverdue',],
    returned: ['Title', 'Borrower', 'DueDate', 'DateReturned', 'Status'],
    reserved: ['Title', 'Borrower', 'DateReserved', 'Status']
};

const columnTemplates = {
    borrowed: ['title', 'borrower', 'borrowedDate', 'dueDate', 'status'],
    overdue: ['title', 'borrower', 'dueDate', 'daysOverdue'],
    returned: ['title', 'borrower', 'dueDate', 'dateReturned', 'status'],
    reserved: ['title', 'borrower', 'dateReserved', 'status']
};

// Shared function to load report
function loadReport() {
    const status = $('#ddlStatus').val().toLowerCase();
    const dateFilter = $('#dateFilterSelector').val() || 'Today';
    const category = $('#categorySelector').val() || 'All';

    if (!columnTemplates[status]) {
        console.warn("Invalid status:", status);
        return;
    }

    $.ajax({
        url: '/Reports/LoadReport',
        method: 'GET',
        data: {
            dateFilter: dateFilter,
            status: status,
            category: category
        },
        beforeSend: function () {
            $('#reportTable tbody').html('<tr><td colspan="10">Loading...</td></tr>');
        },
        success: function (data) {
            const columns = columnHeaderTemplates[status];
            const rowcolumns = columnTemplates[status];
            const $thead = $('#reportTable thead');
            const $tbody = $('#reportTable tbody');

            $thead.empty();
            $tbody.empty();

            // Build header
            let headerRow = '<tr>';
            columns.forEach(col => {
                headerRow += `<th>${col.replace(/([A-Z])/g, ' $1').trim()}</th>`;
            });
            headerRow += '</tr>';
            $thead.append(headerRow);

            // Build body
            if (data.length === 0) {
                $tbody.append(`<tr><td colspan="${rowcolumns.length}">No records found.</td></tr>`);
                return;
            }

            data.forEach(row => {
                let rowHtml = '<tr>';
                rowcolumns.forEach(col => {
                    let value = row[col] ?? '';
                    if (value && typeof value === 'string' && value.includes('T')) {
                        value = new Date(value).toLocaleDateString();
                    }
                    rowHtml += `<td>${value}</td>`;
                });
                rowHtml += '</tr>';
                $tbody.append(rowHtml);
            });
        },
        error: function (xhr, status, error) {
            console.error("AJAX error:", error);
            alert("Failed to load report. Please try again.");
        }
    });
}

// 🔄 Automatically trigger Apply when filters change
$('#dateFilterSelector, #ddlStatus, #categorySelector').on('change', function () {
    loadReport();
});

// Optional helper
function toPascalCaseObject(obj) {
    const newObj = {};
    for (const key in obj) {
        if (obj.hasOwnProperty(key)) {
            const pascalKey = key.charAt(0).toUpperCase() + key.slice(1);
            newObj[pascalKey] = obj[key];
        }
    }
    return newObj;
}

$('#btnDownload').on('click', function () {
    // Fetch table header and body data from DOM
    const headers = [];
    $('#reportTable thead th').each(function () {
        headers.push($(this).text().trim());
    });

    const rows = [];
    $('#reportTable tbody tr').each(function () {
        const row = [];
        $(this).find('td').each(function () {
            row.push($(this).text().trim());
        });
        rows.push(row);
    });

    if (rows.length === 0) {
        alert("No data available to export.");
        return;
    }

    // Initialize jsPDF
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF({
        orientation: 'landscape', // landscape for wide tables
        unit: 'pt',
        format: 'A4'
    });

    // Title and metadata
    const status = $('#ddlStatus').val();
    const dateFilter = $('#dateFilterSelector').val();
    const category = $('#categorySelector').val();

    doc.setFontSize(16);
    doc.text("Library Reports", 40, 40);
    doc.setFontSize(11);
    doc.text(`Status: ${status}   |   Date Range: ${dateFilter}   |   Category: ${category}`, 40, 60);
    doc.text(`Generated on: ${new Date().toLocaleString()}`, 40, 75);

    // Add table
    doc.autoTable({
        head: [headers],
        body: rows,
        startY: 100,
        styles: {
            fontSize: 9,
            halign: 'center',
            valign: 'middle'
        },
        headStyles: {
            fillColor: [243, 121, 157] // pink shade
        },
        alternateRowStyles: {
            fillColor: [245, 245, 245]
        }
    });

    // Download file
    doc.save(`Report_${status}_${new Date().toISOString().slice(0, 10)}.pdf`);
});

$('#btnDownloadCSV').on('click', function () {
    const headers = [];
    $('#reportTable thead th').each(function () {
        headers.push($(this).text().trim());
    });

    const rows = [];
    $('#reportTable tbody tr').each(function () {
        const row = [];
        $(this).find('td').each(function () {
            row.push($(this).text().trim());
        });
        rows.push(row);
    });

    if (rows.length === 0) {
        alert("No data available to export.");
        return;
    }

    // CSV header info (filters)
    const status = $('#ddlStatus').val();
    const dateFilter = $('#dateFilterSelector').val();
    const category = $('#categorySelector').val();
    const generatedDate = new Date().toLocaleString();

    let csvContent = `"Library Reports"\n`;
    csvContent += `"Status:","${status}","Date Range:","${dateFilter}","Category:","${category}"\n`;
    csvContent += `"Generated on:","${generatedDate}"\n\n`;

    // Add table headers
    csvContent += headers.map(h => `"${h}"`).join(',') + '\n';

    // Add table rows
    rows.forEach(row => {
        csvContent += row.map(value => `"${value.replace(/"/g, '""')}"`).join(',') + '\n';
    });

    // Create a Blob and download
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement("a");

    link.href = url;
    link.download = `Report_${status}_${new Date().toISOString().slice(0, 10)}.csv`;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
});