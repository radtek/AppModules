// Инициализация перед использованием:
// Environment.SetEnvironmentVariable("TNS_ADMIN", Environment.CurrentDirectory + "\\ODAC11", EnvironmentVariableTarget.Process);
   Environment.SetEnvironmentVariable("NLS_LANG", "AMERICAN_AMERICA.CL8MSWIN1251", EnvironmentVariableTarget.Process);
   Environment.SetEnvironmentVariable("Path", Environment.CurrentDirectory + "\\ODAC11;" + Environment.GetEnvironmentVariable("Path"), EnvironmentVariableTarget.Process);
   где ODAC11 - директория в которой c instant oracle client

// Параметры:
// Возвращаемый параметр всегда нужно задаваться первым в списке параметров
// Порядок следования параметров должен совпадать с порядком следования в хранимой процедуре
// В возвращаемом (или OUT) параметре типа DBParamString нужно указывать размер строки (параметр size в конструкторе)

// Есть два метода для выполнения команды - Execute и ExecuteSelect:
// - ExecuteSelect предназначен только для выполнения Select-ов (результат выполнения - неименованная выборка, например 'select sysdate from dual' или
//   'select * from all_object'). ExecuteSelect возвращает экземпляр DataTable, содержащий результаты запроса.
// - Execute - для все остальных случаев, когда выполняемый SQL ничего не возвращает, 
//   или возвращает через параметры команды (ReturnValue, Output, InputOutput параметры) 

// Многопоточность: 
// - можно использовать один экземпляр DBManager (а соответственно и DBConnection и DBTransaction) для многих потоков, при этом запросы буду все равно выполняться последовательно
//   следующий начнется, когда закончится предыдущий. Транзакция может быть только одна на все потоки. Можно использовать для фоновых потоков для которых некритичны ожидания
//   окончания предыдущих запросов
// - можно в каждом потоке создать свой экземпляр DBManager и DBConnection.
// - пул соединений на данный момент не поддерживается

// Протестировано со след. версиями:
// БД Oracle - 11.2.0.2.0
// OCI - 11.2.0
// ODP - 2.112.2