-- 1. Procedure to get student count (Connected Mode - Select ONE row with OUT parameter)
CREATE OR REPLACE PROCEDURE GET_STUDENT_COUNT (
    p_course_id IN NUMBER,
    p_count OUT NUMBER
) AS
BEGIN
    SELECT COUNT(*) INTO p_count
    FROM Enrollment
    WHERE CourseID = p_course_id;
END;
/

-- 2. Procedure to get sessions (Connected Mode - Select MULTIPLE rows)
CREATE OR REPLACE PROCEDURE GET_COURSE_SESSIONS (
    p_course_id IN NUMBER,
    p_cursor OUT SYS_REFCURSOR
) AS
BEGIN
    OPEN p_cursor FOR
    SELECT SessionID, CourseID, SessionDate, StartTime, EndTime
    FROM courseSession
    WHERE CourseID = p_course_id;
END;
/
