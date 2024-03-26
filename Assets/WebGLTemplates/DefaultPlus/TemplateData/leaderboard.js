document.addEventListener('DOMContentLoaded', () => {
    const loadButton = document.getElementById('loadButton');

    loadButton.addEventListener('click', async () => {
        const levelInput = document.getElementById('levelInput').value;
        const level = parseInt(levelInput);

        if (level >= 1 && level <= 12) {
            const data = await fetchLeaderboardData(level);
            verifyData(data);
            renderLeaderboard(level, data);
        } else {
            alert('Please enter a valid level between 1 and 12.');
        }
    });

    fetchLeaderboardData(1).then(data => {
        verifyData(data);
        renderLeaderboard(1, data);
    });

    // Function to fetch leaderboard data for a specific level
    async function fetchLeaderboardData(level) {
        try {
            const response = await fetch(`https://game-leaderboard.azurewebsites.net/leaderboard/${level}`);
            if (!response.ok) {
                throw new Error('Failed to fetch data');
            }
            const data = await response.json();
            return data;
        } catch (error) {
            console.error('Error fetching leaderboard data:', error);
            return [];
        }
    }

    // Function to render leaderboard table
    function renderLeaderboard(level, data) {
        const container = document.querySelector('.leaderboard');
        const table = document.createElement('table');
        table.innerHTML = `
            <caption>Level ${level} Leaderboard</caption>
            <thead>
                <tr>
                    <th>Rank</th>
                    <th>Player</th>
                    <th>Time</th>
                </tr>
            </thead>
            <tbody>
                ${data.map(entry => {
            const minutes = Math.floor(entry.time / 60000);
            const seconds = Math.floor((entry.time % 60000) / 1000).toFixed(0).padStart(2, '0');
            const milliseconds = (entry.time % 1000).toFixed(0).padStart(3, '0');
            return `
                    <tr>
                        <td>${entry.rank}</td>
                        <td>${entry.player}</td>
                        <td>${minutes}:${seconds}:${milliseconds}</td>
                    </tr>
                `}).join('')}
            </tbody>
        `;
        container.innerHTML = ''; // Clear existing content
        container.appendChild(table);
    }

    // Check if data only contains letters and numbers, to prevent XSS attacks
    function verifyData(data) {
        for (const entry of data) {
            for (const key in entry) {
                if (typeof entry[key] === 'number') {
                    continue;
                }
                if (typeof entry[key] === 'string' && /^[a-zA-Z0-9:]+$/.test(entry[key])) {
                    continue;
                }
                throw new Error('Invalid data');
            }
        }
    }
});