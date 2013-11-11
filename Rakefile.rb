#Faces v2.7-13_02_2012
require 'rake'
require 'albacore'

#______________________________________________________________________________
#---------------------------------SETTINGS-------------------------------------

# set up the build script folder so we can pull in shared rake scripts.
# This should be the same for most projects, but if your project is a level
# deeper in the repo you will need to add another ..
bs = File.dirname(__FILE__)
bs = File.join(bs, "..") if bs.index("branches") != nil
bs = File.join(bs, "../HabaneroCommunity/BuildScripts")
$buildscriptpath = File.expand_path(bs)
$:.unshift($buildscriptpath) unless
    $:.include?(bs) || $:.include?($buildscriptpath)

if (bs.index("branches") == nil)	
	nuget_version = 'Trunk'
	nuget_version_id = '9.9.999'
	
	$nuget_habanero_version	= nuget_version
	$nuget_smooth_version =	nuget_version
	
	$nuget_publish_version = nuget_version
	$nuget_publish_version_id = nuget_version_id
else
	$nuget_habanero_version	= 'v2.6-13_02_2012'
	$nuget_smooth_version =	'v1.6-13_02_2012'
	
	$nuget_publish_version = 'v2.7-13_02_2012'
	$nuget_publish_version_id = '2.7'
end	

$binaries_baselocation = "bin"
$nuget_baselocation = "nugetArtifacts"
$app_version ='9.9.9.999'
#------------------------build settings--------------------------
require 'rake-settings.rb'

msbuild_settings = {
  :properties => {:configuration => :release},
  :targets => [:clean, :rebuild],
  :verbosity => :quiet,
  #:use => :net35  ;uncomment to use .net 3.5 - default is 4.0
}

#------------------------dependency settings---------------------
#------------------------project settings------------------------
$solution = "source/Habanero.Faces - 2010.sln"
$major_version = ''
$minor_version = ''
$patch_version = ''
#______________________________________________________________________________
#---------------------------------TASKS----------------------------------------

desc "Runs the build all task"
task :default, [:major, :minor, :patch] => [:build_all]

desc "Rakes habanero+smooth, builds Faces"
task :build_all, [:major, :minor, :patch] => [:build_all_nuget]

desc "Rakes habanero+smooth, builds Faces"
task :build_all_nuget, [:major, :minor, :patch] => [:installNugetPackages, :setupversion, :set_assembly_version, :build, :copy_to_nuget, :nuget]

desc "Builds Faces, including tests"
task :build, [:major, :minor, :patch] => [:clean, :setupversion, :set_assembly_version, :msbuild, :test, :copy_to_nuget]

desc "Pushes Faces to Nuget"
task :nuget => [:publishFacesBaseNugetPackage, 
				:publishFacesVWGNugetPackage, 
				:publishFacesWinNugetPackage,
				:publishFacesTestBaseNugetPackage,
				:publishFacesTestWinNugetPackage]
#------------------------Setup Versions---------
desc "Setup Versions"
task :setupversion,:major ,:minor,:patch do |t, args|
	puts cyan("Setup Versions")
	args.with_defaults(:major => "0")
	args.with_defaults(:minor => "0")
	args.with_defaults(:patch => "0000")
	$major_version = "#{args[:major]}"
	$minor_version = "#{args[:minor]}"
	$patch_version = "#{args[:patch]}"
	$app_version = "#{$major_version}.#{$minor_version}.#{$patch_version}.0"
	puts cyan("Assembly Version #{$app_version}")	
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
	FileSystem.ensure_dir_exists $nuget_baselocation
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
    ip.package_names = ["Habanero.Base.#{$nuget_habanero_version}",  
						"Habanero.BO.#{$nuget_habanero_version}",  
						"Habanero.Console.#{$nuget_habanero_version}",  
						"Habanero.DB.#{$nuget_habanero_version}",  
						"Habanero.Test.#{$nuget_habanero_version}",   
						"Habanero.Test.Structure.#{$nuget_habanero_version}",   
						"Habanero.Test.BO.#{$nuget_habanero_version}",   
						"Habanero.Test.DB.#{$nuget_habanero_version}",   
						"Habanero.Smooth.#{$nuget_smooth_version}",
						"Habanero.Naked.#{$nuget_smooth_version}",
						"nunit.Trunk"]
end

desc "Publish the Habanero.Faces.Base nuget package"
pushnugetpackages :publishFacesBaseNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Faces.Base.dll"
  package.Nugetid = "Habanero.Faces.Base.#{$nuget_publish_version}"
  package.Version = $nuget_publish_version_id
  package.Description = "Habanero.Faces.Base"
end

desc "Publish the Habanero.Faces.VWG nuget package"
pushnugetpackages :publishFacesVWGNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Faces.VWG.dll"
  package.Nugetid = "Habanero.Faces.VWG.#{$nuget_publish_version}"
  package.Version = $nuget_publish_version_id
  package.Description = "Habanero.Faces.VWG"
end

desc "Publish the Habanero.Faces.Win nuget package"
pushnugetpackages :publishFacesWinNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Faces.Win.dll"
  package.Nugetid = "Habanero.Faces.Win.#{$nuget_publish_version}"
  package.Version = $nuget_publish_version_id
  package.Description = "Habanero.Faces.Win"
end

desc "Publish the Habanero.Faces.Test.Base nuget package"
pushnugetpackages :publishFacesTestBaseNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Faces.Test.Base.dll"
  package.Nugetid = "Habanero.Faces.Test.Base.#{$nuget_publish_version}"
  package.Version = $nuget_publish_version_id
  package.Description = "Habanero.Faces.Test.Base"
end

desc "Publish the Habanero.Faces.Test.Win nuget package"
pushnugetpackages :publishFacesTestWinNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Faces.Test.Win.dll"
  package.Nugetid = "Habanero.Faces.Test.Win.#{$nuget_publish_version}"
  package.Version = $nuget_publish_version_id
  package.Description = "Habanero.Faces.Test.Win"
end