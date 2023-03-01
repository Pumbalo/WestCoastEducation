const courseList = document.querySelector("#courseList");

const loadVehicles = async () => {
  const url = "http://localhost:5096/api/v1/course";

  const response = await fetch(url);
  console.log(response);

  if (response.status === 200) {
    const data = await response.json();

    let html = "";

    data.map((course) => {
      html += `
                <div>
                    <div>${course.courseTitle}</div>
                </div>
            `;
    });

    courseList.innerHTML = html;
  }
};

loadVehicles();
