fake_run_cmd = dotnet tool run fake run build.fsx -t 

all:
	$(fake_run_cmd) "All"

buildapp:
	$(fake_run_cmd) "BuildApp"

buildtests:
	$(fake_run_cmd) "BuildTests"

runtests:
	$(fake_run_cmd) "RunTests"

rununit:
	$(fake_run_cmd) "RunTests" --unit-only

clean:
	$(fake_run_cmd) "Clean"
