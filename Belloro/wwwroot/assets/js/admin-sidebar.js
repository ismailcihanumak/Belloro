document.addEventListener('DOMContentLoaded', function() {
    const toggleSidebar = document.getElementById('toggle-sidebar');
    const sidebar = document.getElementById('sidebar');
    
    // Sidebar toggle functionality
    toggleSidebar.addEventListener('click', function() {
        sidebar.classList.toggle('collapsed');
        saveSidebarState();
    });
    
    // Load sidebar state from localStorage
    function loadSidebarState() {
        const isCollapsed = localStorage.getItem('sidebarCollapsed') === 'true';
        if (isCollapsed) {
            sidebar.classList.add('collapsed');
        }
    }
    
    // Save sidebar state to localStorage
    function saveSidebarState() {
        const isCollapsed = sidebar.classList.contains('collapsed');
        localStorage.setItem('sidebarCollapsed', isCollapsed);
    }
    
    // Initialize
    loadSidebarState();
});