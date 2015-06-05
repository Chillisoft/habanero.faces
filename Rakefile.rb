#Faces
require "./rake-tasks/init"

#______________________________________________________________________________
#---------------------------------SETTINGS-------------------------------------

$binaries_baselocation = "bin"
$nuget_baselocation = "nugetArtifacts"
$app_version ='9.9.9.999'

#------------------------build settings--------------------------

msbuild_settings = {
    :properties => {:configuration => :release},
    :targets => [:clean, :rebuild],
    :verbosity => :minimal,
    #:use => :net35  ;uncomment to use .net 3.5 - default is 4.0
}

#------------------------dependency settings---------------------
#------------------------project settings------------------------
$solution = "source/Habanero.Faces - 2010.sln"
$solutionNuget = '"source/Habanero.Faces - 2010.sln"'
$major_version = ''
$minor_version = ''
$patch_version = ''
$nuget_apikey = ''
$nuget_sourceurl = ''
$nuget_publish_version = 'Trunk'
#______________________________________________________________________________
#---------------------------------TASKS----------------------------------------
desc "Runs the build all task"
task :default, [:major, :minor, :patch] => [:setupvars, :build]

desc "Pulls habanero deps from local nuget, builds, tests and pushes faces"
task :build_test_push_internal, [:major, :minor, :patch, :apikey, :sourceurl] => [:build_internal, :nugetpush]

desc "Pulls habanero deps from local nuget, builds and tests"
task :build_internal, [:major, :minor, :patch, :sourceurl] => [:setupvars, :installNugetPackages, :build]

desc "Builds Testability, including tests"
task :build, [:major, :minor, :patch]  => [:clean, :update_packages, :set_assembly_version, :msbuild, :copy_to_nuget, :test]

#------------------------Setup Versions---------
desc "Setup Variables"
task :setupvars, :major, :minor, :patch, :apikey, :sourceurl do |t, args|
    puts cyan("Setup Variables")
    args.with_defaults(:major => "0")
    args.with_defaults(:minor => "0")
    args.with_defaults(:patch => "0000")
    args.with_defaults(:apikey => "")
    args.with_defaults(:sourceurl => "")
    $major_version = "#{args[:major]}"
    $minor_version = "#{args[:minor]}"
    $patch_version = "#{args[:patch]}"
    $nuget_apikey = "#{args[:apikey]}"
    $nuget_sourceurl = "#{args[:sourceurl]}"
    $app_version = "#{$major_version}.#{$minor_version}.#{$patch_version}.0"
    puts cyan("Assembly Version #{$app_version}")
    puts cyan("Nuget key: #{$nuget_apikey} for: #{$nuget_sourceurl}")
end

desc "Updates packages with nuget"
updatenugetpackages :update_packages do |nuget|
    puts cyan("Updating nuget packages")
    nuget.solution = $solutionNuget
end

task :set_assembly_version do
    puts green("Setting Shared AssemblyVersion to: #{$app_version}")
    file_path = "source/Common/AssemblyInfoShared.cs"
    outdata = File.open(file_path).read.gsub(/"9.9.9.999"/, "\"#{$app_version}\"")
    File.open(file_path, 'w') do |out|
        out << outdata
    end    
end
#------------------------build Faces  --------------------

desc "Cleans the bin folder"
task :clean do
    puts cyan("Cleaning bin folder")
    FileUtils.rm_rf 'bin'
    FileUtils.rm_rf $nuget_baselocation    
end

desc "Builds the solution with msbuild"
msbuild :msbuild do |msb| 
    puts cyan("Building #{$solution} with msbuild")
    msb.update_attributes msbuild_settings
    msb.solution = $solution
end

desc "Runs the tests"
nunit :test do |nunit|
    puts cyan("Running tests")
    nunit.assemblies 'bin\Habanero.Faces.Test.Win.dll',
                     'bin\Habanero.Faces.Test.VWG.dll',
                     'bin\Habanero.Faces.Test.Base.dll'
end

def copy_nuget_files_to location
    FileSystem.ensure_dir_exists "#{location}/"
    FileUtils.cp "#{$binaries_baselocation}/Habanero.Faces.Base.dll", location
    FileUtils.cp "#{$binaries_baselocation}/Habanero.Faces.VWG.dll", location
    FileUtils.cp "#{$binaries_baselocation}/Habanero.Faces.Win.dll", location
    FileUtils.cp "#{$binaries_baselocation}/Habanero.Faces.Test.Base.dll", location
    FileUtils.cp "#{$binaries_baselocation}/Habanero.Faces.Test.Win.dll", location
end

task :copy_to_nuget do
    puts cyan("Copying files to the nuget folder")    
    copy_nuget_files_to $nuget_baselocation
end

desc "Install nuget packages"
getnugetpackages :installNugetPackages do |ip|
    ip.package_names = ["Habanero.Base.#{$nuget_publish_version}",  
                        "Habanero.BO.#{$nuget_publish_version}",  
                        "Habanero.Console.#{$nuget_publish_version}",  
                        "Habanero.DB.#{$nuget_publish_version}",  
                        "Habanero.Test.#{$nuget_publish_version}",   
                        "Habanero.Test.Structure.#{$nuget_publish_version}",   
                        "Habanero.Test.BO.#{$nuget_publish_version}",   
                        "Habanero.Test.DB.#{$nuget_publish_version}",   
                        "Habanero.Smooth.#{$nuget_publish_version}",
                        "Habanero.Naked.#{$nuget_publish_version}"]
    ip.SourceUrl = "#{$nuget_sourceurl}/nuget"
    ip.Version = $app_version
end

desc "Pushes Faces to Nuget"
task :nugetpush => [:publishFacesBaseNugetPackage, 
                    :publishFacesVWGNugetPackage, 
                    :publishFacesWinNugetPackage,
                    :publishFacesTestBaseNugetPackage,
                    :publishFacesTestWinNugetPackage]
                
desc "Publish the Habanero.Faces.Base nuget package"
pushnugetpackagesonline :publishFacesBaseNugetPackage do |package|
    package.InputFileWithPath = "bin/Habanero.Faces.Base.dll"
    package.Nugetid = "Habanero.Faces.Base.#{$nuget_publish_version}"
    package.Version = $app_version
    package.Description = "Habanero.Faces.Base"
    package.ApiKey = "#{$nuget_apikey}"
    package.SourceUrl = "#{$nuget_sourceurl}"
end

desc "Publish the Habanero.Faces.VWG nuget package"
pushnugetpackagesonline :publishFacesVWGNugetPackage do |package|
    package.InputFileWithPath = "bin/Habanero.Faces.VWG.dll"
    package.Nugetid = "Habanero.Faces.VWG.#{$nuget_publish_version}"
    package.Version = $app_version
    package.Description = "Habanero.Faces.VWG"
    package.ApiKey = "#{$nuget_apikey}"
    package.SourceUrl = "#{$nuget_sourceurl}"
end

desc "Publish the Habanero.Faces.Win nuget package"
pushnugetpackagesonline :publishFacesWinNugetPackage do |package|
    package.InputFileWithPath = "bin/Habanero.Faces.Win.dll"
    package.Nugetid = "Habanero.Faces.Win.#{$nuget_publish_version}"
    package.Version = $app_version
    package.Description = "Habanero.Faces.Win"
    package.ApiKey = "#{$nuget_apikey}"
    package.SourceUrl = "#{$nuget_sourceurl}"
end

desc "Publish the Habanero.Faces.Test.Base nuget package"
pushnugetpackagesonline :publishFacesTestBaseNugetPackage do |package|
    package.InputFileWithPath = "bin/Habanero.Faces.Test.Base.dll"
    package.Nugetid = "Habanero.Faces.Test.Base.#{$nuget_publish_version}"
    package.Version = $app_version
    package.Description = "Habanero.Faces.Test.Base"
    package.ApiKey = "#{$nuget_apikey}"
    package.SourceUrl = "#{$nuget_sourceurl}"
end

desc "Publish the Habanero.Faces.Test.Win nuget package"
pushnugetpackagesonline :publishFacesTestWinNugetPackage do |package|
    package.InputFileWithPath = "bin/Habanero.Faces.Test.Win.dll"
    package.Nugetid = "Habanero.Faces.Test.Win.#{$nuget_publish_version}"
    package.Version = $app_version
    package.Description = "Habanero.Faces.Test.Win"
    package.ApiKey = "#{$nuget_apikey}"
    package.SourceUrl = "#{$nuget_sourceurl}"
end
