#Faces branch v2.7
require 'rake'
require 'albacore'

#______________________________________________________________________________
#---------------------------------SETTINGS-------------------------------------

# set up the build script folder so we can pull in shared rake scripts.
# This should be the same for most projects, but if your project is a level
# deeper in the repo you will need to add another ..
bs = File.dirname(__FILE__)
bs = File.join(bs, "..") if bs.index("branches") != nil
bs = File.join(bs, "../../../HabaneroCommunity/BuildScripts")
$buildscriptpath = File.expand_path(bs)
$:.unshift($buildscriptpath) unless
    $:.include?(bs) || $:.include?($buildscriptpath)

#------------------------build settings--------------------------
require 'rake-settings.rb'

msbuild_settings = {
  :properties => {:configuration => :release},
  :targets => [:clean, :rebuild],
  :verbosity => :quiet,
  #:use => :net35  ;uncomment to use .net 3.5 - default is 4.0
}

#------------------------dependency settings---------------------
$habanero_version = 'branches/v2.6'
require 'rake-habanero.rb'

$smooth_version = 'branches/v1.6'
require 'rake-smooth.rb'

#------------------------project settings------------------------
$solution = "source/Habanero.Faces - 2010.sln"

#______________________________________________________________________________
#---------------------------------TASKS----------------------------------------

desc "Runs the build all task"
task :default => [:build_all_nuget]

desc "Rakes habanero+smooth, builds Faces"
task :build_all_nuget => [:create_temp, :installNugetPackages, :rake_habanero, :rake_smooth, :build, :delete_temp]

desc "Builds Faces, including tests"
task :build => [:clean, :updatelib, :installNugetPackages, :msbuild, :test, :commitlib]

desc "Pushes Faces to Nuget"
task :nuget => [:publishFacesBaseNugetPackage, :publishFacesVWGNugetPackage, :publishFacesWinNugetPackage]

#------------------------build Faces  --------------------

desc "Cleans the bin folder"
task :clean do
	puts cyan("Cleaning bin folder")
	FileUtils.rm_rf 'bin'
end

svn :update_lib_from_svn do |s|
	s.parameters "update lib"
end

task :updatelib => :update_lib_from_svn do 
	puts cyan("Updating lib")
	FileUtils.cp Dir.glob('temp/bin/Habanero.Base.dll'), 'lib'
	FileUtils.cp Dir.glob('temp/bin/Habanero.Base.pdb'), 'lib'
	FileUtils.cp Dir.glob('temp/bin/Habanero.Base.xml'), 'lib'
	FileUtils.cp Dir.glob('temp/bin/Habanero.BO.dll'), 'lib'
	FileUtils.cp Dir.glob('temp/bin/Habanero.BO.pdb'), 'lib'
	FileUtils.cp Dir.glob('temp/bin/Habanero.BO.xml'), 'lib'
	FileUtils.cp Dir.glob('temp/bin/Habanero.Console.dll'), 'lib'
	FileUtils.cp Dir.glob('temp/bin/Habanero.Console.pdb'), 'lib'
	FileUtils.cp Dir.glob('temp/bin/Habanero.Console.xml'), 'lib'
	FileUtils.cp Dir.glob('temp/bin/Habanero.DB.dll'), 'lib'
	FileUtils.cp Dir.glob('temp/bin/Habanero.DB.pdb'), 'lib'
	FileUtils.cp Dir.glob('temp/bin/Habanero.DB.xml'), 'lib'
	FileUtils.cp Dir.glob('temp/bin/Habanero.Test.BO.dll'), 'lib'
	FileUtils.cp Dir.glob('temp/bin/Habanero.Test.BO.pdb'), 'lib'
	FileUtils.cp Dir.glob('temp/bin/Habanero.Test.DB.dll'), 'lib'	
	FileUtils.cp Dir.glob('temp/bin/Habanero.Test.DB.pdb'), 'lib'	
	FileUtils.cp Dir.glob('temp/bin/Habanero.Test.dll'), 'lib'	
	FileUtils.cp Dir.glob('temp/bin/Habanero.Test.pdb'), 'lib'	
	FileUtils.cp Dir.glob('temp/bin/Habanero.Test.Structure.dll'), 'lib'	
	FileUtils.cp Dir.glob('temp/bin/Habanero.Test.Structure.pdb'), 'lib'	
	
	FileUtils.cp Dir.glob('temp/bin/Habanero.Smooth.dll'), 'lib'	
	FileUtils.cp Dir.glob('temp/bin/Habanero.Smooth.pdb'), 'lib'	
	FileUtils.cp Dir.glob('temp/bin/Habanero.Naked.dll'), 'lib'	
	FileUtils.cp Dir.glob('temp/bin/Habanero.Naked.pdb'), 'lib'	
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
	nunit.assemblies 'bin\Habanero.Faces.Test.Win.dll','bin\Habanero.Faces.Test.VWG.dll','bin\Habanero.Faces.Test.Base.dll'
end

svn :commitlib do |s|
	puts cyan("Commiting lib")
	s.parameters "ci lib -m autocheckin"
end

desc "Install nuget packages"
getnugetpackages :installNugetPackages do |ip|
    ip.package_names = ["Habanero.Base.V2.6", \
                        "Habanero.BO.V2.6", \
                        "Habanero.Console.V2.6", \
                        "Habanero.DB.V2.6", \
                        "Habanero.Smooth.v1.6", \
                        "Habanero.Naked.v1.6", \
                        "Habanero.Testability.v1.3", \
                        "Habanero.Testability.Helpers.v1.3", \
                        "Habanero.Testability.Testers.v1.3"]
end

desc "Publish the Habanero.Faces.Base nuget package"
pushnugetpackages :publishFacesBaseNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Faces.Base.dll"
  package.Nugetid = "Habanero.Faces.Base.V2.6_2011-08-24"
  package.Version = "2.6"
  package.Description = "Habanero.Faces.Base"
end

desc "Publish the Habanero.Faces.VWG nuget package"
pushnugetpackages :publishFacesVWGNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Faces.VWG.dll"
  package.Nugetid = "Habanero.Faces.VWG.V2.6_2011-08-24"
  package.Version = "2.6"
  package.Description = "Habanero.Faces.VWG"
end

desc "Publish the Habanero.Faces.Win nuget package"
pushnugetpackages :publishFacesWinNugetPackage do |package|
  package.InputFileWithPath = "bin/Habanero.Faces.Win.dll"
  package.Nugetid = "Habanero.Faces.Win.V2.6_2011-08-24"
  package.Version = "2.6"
  package.Description = "Habanero.Faces.Win"
end
